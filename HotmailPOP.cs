using System.Diagnostics;

namespace iMacrosPostingDashboard
{
    using OpenPop.Mime;
    using OpenPop.Pop3;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class HotmailPOP
    {


        private int email_num;
        private string linkpattern;
        private string cSender;
        private int ShiftBy = 0;

        // private static string LinkExtracted;

        public HotmailPOP()
        {
        }


        private List<Message> FetchUnseenMessages(string hostname, int port, bool useSsl, string username, string password, List<string> seenUids)
        {
            List<Message> list3;
            List<Message> list = new List<Message>();
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    int num;
                    client.Connect(hostname, port, useSsl);
                    client.Authenticate(username, password);
                    List<string> messageUids = client.GetMessageUids();
                    if (messageUids.Count > 0)
                    {
                        num = messageUids.Count;
                    }
                    else
                    {
                        Console.Write("No Emails found in that Email account");
                        num = 0;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        string item = messageUids[i];
                        if (!seenUids.Contains(item))
                        {
                            Message message = client.GetMessage(i + 1);
                            list.Add(message);
                            seenUids.Add(item);
                        }
                    }
                    list3 = list;
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                list3 = list;
            }
            return list3;
        }

        private string FindPlainTextInMessage(Message message)
        {
            OpenPop.Mime.Header.MessageHeader msgheader = message.Headers;
            string sender = msgheader.From.Address;
            string LinkExtracted = "";
            if (sender == cSender)
            {
                List<MessagePart> list = message.FindAllTextVersions();
                string str = "";
                foreach (MessagePart part in list)
                {
                    if (part != null)
                    {
                        try
                        {
                            //string pattern = @"http://www.gutefrage.net/registrierungsbestaetigung.*(?=\042)";  linkpattern
                            //part.Save(new FileInfo("temp"));
                            //string str2 = File.ReadAllText("temp");
                            string str2 = part.GetBodyAsText();
                            int startIndex = 0;
                            int num2 = 0;
                            startIndex = str2.IndexOf(linkpattern, startIndex);
                            while (startIndex != -1)
                            {
                                startIndex = startIndex + ShiftBy;
                                char[] anyOf = new char[] { ' ', '"', '>', '<', '\r', '\n', '\\', ')' };
                                num2 = str2.IndexOfAny(anyOf, startIndex);
                                string str3 = str2.Substring(startIndex, num2 - startIndex);
                                if (str == str3)
                                {
                                    startIndex = str2.IndexOf(linkpattern, num2);
                                }
                                else
                                {
                                    // File.AppendAllText("links.txt", str3 + "\r\n");
                                    LinkExtracted = str3;
                                    str = str3;
                                    startIndex = str2.IndexOf(linkpattern, num2);
                                }
                            }
                            return LinkExtracted;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Pizdets!");
                            return null;
                        }
                    }
                }
            }
            if (LinkExtracted != "")
            {
                return LinkExtracted;
            }
            else
            {
                return null;
            }
        }

        public string ExtractConfLink(string email, string pwd, string linkstructure, string sender, int ShiftLinkBy)
        {
            // Send variables to ExtractConfLink class
            // Return LinkExtracted
            string LinkExtracted = "";
            

            if (email != "" && pwd != "")
            {
                linkpattern = linkstructure;
                cSender = sender;
                ShiftBy = ShiftLinkBy;
                LinkExtracted = POP(email, pwd);
            }
            
            return LinkExtracted;
        }


        /* private static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                string user = args[0];
                string pass = args[1];
                POP(user, pass);
            }
        }

        */
 
        private string POP(string user, string pass)
        {
            string LinkExtracted = "";
            try
            {
                List<Message> list = new List<Message>();
                new List<string>();
                foreach (Message message in FetchUnseenMessages("pop3.live.com", 0x3e3, true, user, pass, new List<string>()))
                {
                    LinkExtracted = FindPlainTextInMessage(message);
                    if (LinkExtracted != "" && LinkExtracted != null) break;
                    email_num++;
                }
                return LinkExtracted;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}