using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using MyGeneration.dOOdads;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using NUnit.Framework;



namespace iMacrosPostingDashboard
{
    public class GenericPoster
    {

        #region Set all Class-wide variables

        public bool RunningStatus = false;  //  0 - not running,  1 - running

        public DateTime StartTime = Convert.ToDateTime("09:03:00 AM");
        public DateTime EndTime = Convert.ToDateTime("10:00:00 PM");
        public int PauseBtwPosts = 30;
        string passwd = "";
        private bool confirmed = false;

        Dictionary<string, string> property = new Dictionary<string, string>();

        iMacrosPostReturnVars poster;

        private bool postQnA = false;
        private bool questionposted = true;

        private BackgroundWorker worker;
        private Projects proj;
        private Emailaccounts tblaccts;
        private Proxies tblproxies;
        private Responses tblresp;
        private Affiliateprograms tblaff;
        private Emailaccounts tblaccts_copy;

        private StdPosterFunctionsNonStatic stdfunc;

        private string iMacrosCreateAccountCode = "";
        private string iMacrosLoginAndPostCode = "";

        private int nexttmpl = 0;
        private int nextproxy = 0;
        private int nextemail = 0;
        private int nexttopic = 0;

        int proxycount = 0;

        int progressvalue = 2; // a variable for reporting % to the progressBar

        private string GeneratedResponse = "";

        // Project Specific Variables
        private Topicsgeneric tbltopics;
        private string ProjectName = "";
        private string AnswerTemplate = "";
        
        private string kwd = "";

        private int tzoneproject = 0;

        public DoWorkEventArgs error;

        #endregion

        // Constructors:
        public GenericPoster(ref Object sender, ref DoWorkEventArgs e, string projname, string topictablename, string AnswerTmpl)
        {
            // INITialize all DB objects
            worker = sender as BackgroundWorker;
            proj = new Projects();
            tblaccts = new Emailaccounts();
            tblproxies = new Proxies();
            tblresp = new Responses();
            tblaff = new Affiliateprograms();
            tbltopics = new Topicsgeneric(topictablename);

            stdfunc = new StdPosterFunctionsNonStatic();

            nexttmpl = 0;
            nextproxy = 0;
            nextemail = 0;
            nexttopic = 0;

            ProjectName = projname;
            AnswerTemplate = AnswerTmpl;
            


            //  worker.ReportProgress((2 * 1 * 10), "Testing...");

            error = e;
            e = error;
            // error.Cancel = true;
            
            SelectProject(ref proj, ProjectName);
            ParseProperties();

            if (property.ContainsKey("projecttimezone"))
                try
                {
                    tzoneproject = Convert.ToInt32(property["projecttimezone"]);
                }
                catch
                {
                }

            if ((worker.CancellationPending == true))  // if the STOP button has been clicked
            {
                e.Cancel = true;
                return;
            }
        }
        public GenericPoster()
        {
            // For testing purposes
        }

        // HELPERS, can REMAIN PRIVATE :
        private bool CancellationIsPending()
        {
            if ((worker.CancellationPending == true))  // if the STOP button has been clicked
            {
                error.Cancel = true;
                return true;
            }
            else if (error.Cancel == true)
                return true;
            else return false;
        }
        private void EmailKwdStatus(bool Anykwdsleft)
        {
            // READ FROM CONFIG FILE 
            string fromSender = (string)ConfigurationManager.AppSettings["Sender"];
            string toReceiver = (string)ConfigurationManager.AppSettings["Receiver"];
            string SenderPass = (string)ConfigurationManager.AppSettings["SenderPass"];
            
            // STANDARD SMTP SENDER CODE 
            var fromAddress = new MailAddress(fromSender, "Forumu Statusas");
            var toAddress = new MailAddress(toReceiver, "");
            string fromPassword = SenderPass;
            string subject5orless = "Priminimas: forume " + ProjectName + " liko 5 raktažodžiai";
            string subjectNone = "Priminimas: forume " + ProjectName + " visai nebeliko raktažodžių";
            string subject = "";

            string body = @"Reiktų atrinkti daugiau raktažodžių šiam forumui: " + ProjectName + "\n\n" +
                            "Nuoroda: http://eglesum.com/forumposter/ForumDash/public/forumdash/edit/" + proj.Id.ToString() + " \n";

            if (Anykwdsleft) subject = subject5orless;
            else subject = subjectNone;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        private void SleepIfNighttime()  // ONLY works if both StartTime and EndTime are in the same day
        {
            if (IsNightTime(StartTime, EndTime, tzoneproject)) worker.ReportProgress((2 * progressvalue), "Night time sleeping until: " + StartTime.ToShortTimeString());

            while (IsNightTime(StartTime, EndTime, tzoneproject))
            {
                //
                System.Threading.Thread.Sleep((1000 * 60 * 10));
            }

        }
        private void ParseProperties()
        {
            property = new Dictionary<string, string>();
            if (proj.Properties != "" && proj.Properties != null)
            {
                try
                {
                    property = JsonConvert.DeserializeObject<Dictionary<string, string>>(proj.Properties);
                }
                catch
                {
                    property = new Dictionary<string, string>();
                    property.Add("empty", "empty");
                }
            }
            else
            {
                property = new Dictionary<string, string>();
                property.Add("empty", "empty");
            }
            if (property == null)
            {
                property = new Dictionary<string, string>();
                property.Add("empty", "empty");
            }
        }

        // TESTABLE METHODS :
        public bool IsNightTime(DateTime Start, DateTime End, int tzone)
        {
            //

            if ((DateTime.Now.AddHours(tzone).TimeOfDay < Start.TimeOfDay) || (DateTime.Now.AddHours(tzone).TimeOfDay > End.TimeOfDay))
                return true;
            else return false;
        }
        private void SelectProject(ref Projects proj_local, string ProjectName_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Initialized. Selecting the project. ");
            int counter1 = 0;
            SelectProjectAgain:
                proj_local.Where.ProjectName.Value = ProjectName_local;
                proj_local.Where.ProjectName.Operator = WhereParameter.Operand.Equal;
                try
                {
                    proj_local.Query.Load();
                }
                catch
                {
                    counter1++;
                    if (counter1 >= 4) return;
                    goto SelectProjectAgain;
                }
                worker.ReportProgress((2 * progressvalue++), "Project selected. ");            
        }
        private void SaveMacroCodes()
        {
            iMacrosCreateAccountCode = proj.ImacrosCreateAccCode;
            iMacrosLoginAndPostCode = proj.ImacrosLoginPost;
        }
        private int NumberOfKwdsRemaining()
        {
            int KwdNmbr = 0;

            nexttopic = proj.LastTopicUsed + 1;

            tbltopics.Where.WhereClauseReset();

            tbltopics.Where.ProductKeyword.Value = string.Empty;
            tbltopics.Where.ProductKeyword.Operator = WhereParameter.Operand.NotEqual;
            WhereParameter wp = tbltopics.Where.TearOff.ProductKeyword;
            wp.Operator = WhereParameter.Operand.IsNotNull;
         
            tbltopics.Where.Id.Value = nexttopic;
            tbltopics.Where.Id.Operator = WhereParameter.Operand.GreaterThanOrEqual;

            tbltopics.Aggregate.Id.Function = AggregateParameter.Func.Count;
            tbltopics.Aggregate.Id.Alias = "Kwdsleft";

            tbltopics.Query.Load();
            DataView dv = tbltopics.DefaultView;
            // tbltopics.Query.ReturnReader();

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                try
                {
                    KwdNmbr = (int)(long)row[0];   // Because it's an aggregate function, it should be only one row
                }
                catch
                {
                    // 
                }
            }
            string lastq = tbltopics.Query.LastQuery;
            tbltopics.Aggregate.AggregateClauseReset();
            tbltopics.Where.WhereClauseReset();
            return KwdNmbr;
        }
        private void SelectNextTopic()
        {
            worker.ReportProgress((2 * progressvalue ), "Selecting topic. ");
            int counter2 = 0;
            kwd = "";
            
                nexttopic = proj.LastTopicUsed + 1; // Takes the next topic, as per the Projects table

                int MaxTopics = 0; 
                tbltopics.Aggregate.Id.Function = AggregateParameter.Func.Max; 
                tbltopics.Aggregate.Id.Alias = "Max topics"; 

                tbltopics.Query.Load(); 
                DataView dv = tbltopics.DefaultView; 
                // tbltopics.Query.ReturnReader(); 

                foreach (DataRowView rowView in dv)
                {
                    DataRow row = rowView.Row;
                    MaxTopics = (int)row[0];   // Figure out the number of rows in that table
                }

                tbltopics.Aggregate.AggregateClauseReset();

        SelectTopicAgain:
                if (nexttopic > MaxTopics)
                {
                    worker.ReportProgress((2 * progressvalue), "Out of topics.");
                    error.Cancel = true;
                    return;
                }
    
                tbltopics.Where.Id.Value = nexttopic;
                tbltopics.Where.Id.Operator = WhereParameter.Operand.Equal;
                
                try
                {
                    if (!tbltopics.Query.Load()) // if the Query Result is empty, try to select the next row
                    {
                        nexttopic++;
                        goto SelectTopicAgain;
                    }
                    if (tbltopics.PostedStatus == 1) // if the topic is already posted, select the next topic
                    {
                        nexttopic++;
                        goto SelectTopicAgain;
                    }
                    if (tbltopics.ProductKeyword == "0") // if the keyword is "0", select the next topic
                    {
                        nexttopic++;
                        goto SelectTopicAgain;
                    }

                }
                catch
                {
                    counter2++;
                    if (counter2 >= 4) return;
                    goto SelectTopicAgain;
                }
            
            kwd = tbltopics.ProductKeyword;
            worker.ReportProgress((2 * progressvalue), "Topic selected. ");
        }
        private void WaitForKeywordsOrShutTheProcessDown()
        {
            if (kwd == "" || kwd == null)
            {
                try
                {
                    EmailKwdStatus(false);
                }
                catch
                { 
                }
            }

            while (kwd == "" || kwd == null)
            {
                worker.ReportProgress((2 * progressvalue), "No keyword found. ");
                worker.ReportProgress((2 * progressvalue), "Sleeping 5 min. and checking for kwds again.");
                
                System.Threading.Thread.Sleep((1000 * 60 * 5));

                if (CancellationIsPending())
                    return;
                SelectNextTopic();
            }
        }
        private int SelectNextEmail(ref Emailaccounts tblaccts_local, int LastAccountUsed_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Selecting email account.");
            int counter3 = 0;

            int MaxEmails = 0;
            int nextemail_local = LastAccountUsed_local + 1; // Filter the Email Account
            int firstemail;
            int lastemail;
            
            if (property.ContainsKey("firstemail") && property.ContainsKey("lastemail"))
            {
                try
                {
                    firstemail = Convert.ToInt32(property["firstemail"]);
                    lastemail = Convert.ToInt32(property["lastemail"]);
                    if (nextemail_local < firstemail)
                        nextemail_local = firstemail;
                    if (nextemail_local > lastemail)
                        nextemail_local = firstemail;
                }
                catch
                {
                }
            }
            

            tblaccts_local.Where.WhereClauseReset();
            tblaccts_local.Aggregate.Id.Function = AggregateParameter.Func.Max;
            tblaccts_local.Aggregate.Id.Alias = "Max emails";

            tblaccts_local.Query.Load();
            DataView dv = tblaccts_local.DefaultView;
            // tbltopics.Query.ReturnReader();

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                MaxEmails = (int)row[0];
            }

            SelectEmailAgain:
                if (nextemail_local > MaxEmails)
                {
                    worker.ReportProgress((2 * progressvalue), "Out of email accounts.");
                    error.Cancel = true;
                    return 0;
                }

                tblaccts_local.Aggregate.AggregateClauseReset();
                tblaccts_local.Where.Id.Value = nextemail_local;
                tblaccts_local.Where.Id.Operator = WhereParameter.Operand.Equal;
                try
                {
                    if (!tblaccts_local.Query.Load())
                    {
                        nexttopic++;
                        goto SelectEmailAgain;
                    }
                
                }
                catch
                {
                    counter3++;
                    if (counter3 >= 4)
                    {
                        error.Cancel = true;
                        return 0;
                    }
                    goto SelectEmailAgain;
                }
                worker.ReportProgress((2 * progressvalue++), "Email selected. ");
                return nextemail_local;
            // string CurrentEmail = tblaccts.Email;
        }
        private int SelectNextProxy(ref Proxies tblproxies_local, int LastProxyUsed_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Selecting proxy. ");

            int counter4 = 0;
            SelectProxyAgain:

                int nextproxy_local = LastProxyUsed_local + 1;
                try
                {
                    tblproxies_local.LoadAll();
                }
                catch
                {
                    counter4++;
                    if (counter4 >= 4) return 0;
                    goto SelectProxyAgain;
                }
                
                proxycount = tblproxies_local.RowCount;
                if (nextproxy_local > proxycount)
                {
                    nextproxy_local = 1;
                }

                tblproxies_local.Where.Id.Value = nextproxy_local;
                tblproxies_local.Where.Id.Operator = WhereParameter.Operand.Equal;

                try
                {
                    tblproxies_local.Query.Load();
                }
                catch
                {
                    counter4++;
                    if (counter4 >= 4) return 0;
                    goto SelectProxyAgain;
                }
                worker.ReportProgress((2 * progressvalue++), "Proxy selected.");
                return nextproxy_local;
        }
        private void CheckIfProxyIsWorkingSelectNextIfNotWorking()
        {
            int loopcounter = 0;
            worker.ReportProgress((2 * progressvalue++), "Checking if proxy is alive.");
            Httpcalls httpcall = new Httpcalls();

            if (proxycount == 0)
                nextproxy = SelectNextProxy(ref tblproxies, proj.LastProxyUsed);
            if (proxycount == 0)
            {
                worker.ReportProgress((2 * progressvalue++), "Failed selecting proxies.");
                error.Cancel = true;
                return;
            }

            while (!httpcall.IsAlive(tblproxies.Proxy))
            {
                tblproxies.Active = 0;  //  Mark DEAD proxy as dead
                tblproxies.Save();  //  Save that value

                nextproxy = nextproxy + 1;  // select next proxy in the list
                if (nextproxy > proxycount)
                {
                    nextproxy = 1;
                }

                tblproxies.Where.Id.Value = nextproxy;  
                tblproxies.Where.Id.Operator = WhereParameter.Operand.Equal;
                try
                {
                    tblproxies.Query.Load();
                }
                catch
                {
                }
                loopcounter++;
                if (loopcounter > proxycount)
                {
                    worker.ReportProgress((2 * progressvalue++), "No working proxies found.");
                    error.Cancel = true;
                    return;
                }
                worker.ReportProgress((2 * progressvalue), "Trying the next proxy.");
            }  // end while !IsAlive(proxy)

            tblproxies.Active = 1;
            try
            {
                tblproxies.Save();
            }
            catch
            {
            }
            worker.ReportProgress((2 * progressvalue++), "Proxy working well.");

        } // end  CHECK IF PROXY IS WORKING, SELECT NEXT IF NOT WORKING
        private bool CreateNewAccount(Emailaccounts tblaccts_local, string proxy_local, string createcode_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Creating a new account.");

            passwd = tblaccts_local.Password; // GLOBAL variable to track the changes in password
            int hascaptcha = 0;
            if (property.ContainsKey("captcha"))
                try
                {
                    hascaptcha = Convert.ToInt32(property["captcha"]);
                }
                catch { }

            if (hascaptcha == 1)
            {
                CreateNewAccountWithCaptcha(tblaccts_local, proxy_local, createcode_local);
                return true;
            }
            else
            {
                tblaccts_copy = tblaccts_local;
                if (stdfunc.CreateAccount(tblaccts_copy, proxy_local, createcode_local))
                {
                    worker.ReportProgress((2 * progressvalue++), "Account created.");
                    return true;
                }
                else return false;
            }
        }
        private void CreateNewAccountWithCaptcha(Emailaccounts tblaccts_local, string proxy_local, string createcode_local)
        {
            tblaccts_copy = tblaccts_local;
            string[] ErrorAndId = stdfunc.CreateAccountWithCaptcha(tblaccts_copy, proxy_local, createcode_local);
            //string[] ErrorAndId = { "", "" };
            string ErrorMsg = "";
            ErrorMsg = ErrorAndId[0];
            string CaptchaId = ErrorAndId[1];
            int br = 0;
            while (ErrorMsg != "" && ErrorMsg != "#EANF#" && ErrorMsg != null)
            {
                // Blogas CAPTCHA
                // Report bad CAPTCHA
                if (CancellationIsPending()) return;
                if (CaptchaId != "NODATA") stdfunc.ReportBadCaptcha(CaptchaId);
                ErrorAndId = stdfunc.CreateAccountWithCaptcha(tblaccts_copy, proxy_local, createcode_local);
                ErrorMsg = ErrorAndId[0];
                CaptchaId = ErrorAndId[1];
                br++;
                if (br > 5)
                {
                    worker.ReportProgress((2 * progressvalue++), "6 captchas solved incorrectly. Stopping.");
                    //e.Cancel = true;
                    break;
                }
            }
        }
        private void WaitSomeTime(int waitminutes) // Need a parameter of time
        {
            worker.ReportProgress((2 * progressvalue++), "Account created. Pausing " + waitminutes + " min.");
            System.Threading.Thread.Sleep((1000 * 60 * waitminutes));
        }
        private bool ConfirmNewAccount(string passwordlinkstructure = "", int passwordshiftby = 0)
        {
            worker.ReportProgress((2 * progressvalue++), "Confirming account.");
            passwd = tblaccts.Password;
            string linkstructure;
            int shiftby;

            // The following checks, if this is a RARE case with a second email that contains the password
            if (passwordlinkstructure == "")
            {
                linkstructure = proj.Linkstructure.ToString();
                shiftby = proj.ShiftLinkstructureBy;
            }
            else
            {
                linkstructure = passwordlinkstructure;
                shiftby = passwordshiftby;
            }

            if (proj.Senderemail == "" || proj.Senderemail == null)
            {
                return true;
            }
            else
            {

                if (stdfunc.ConfirmAccount(tblaccts.Email, ref passwd, linkstructure, proj.Senderemail, tblproxies.Proxy, shiftby))
                {
                    worker.ReportProgress((2 * progressvalue++), "Account confirmed.");
                    // tblaccts.Password = passwd;
                    // tblaccts.Save();
                    return true;
                }
                else
                {
                    worker.ReportProgress((2 * progressvalue++), "Unconfirmed. Waiting extra 5 min.");
                    System.Threading.Thread.Sleep((1000 * 60 * 5));

                    if (stdfunc.ConfirmAccount(tblaccts.Email, ref passwd, linkstructure, proj.Senderemail, tblproxies.Proxy, shiftby))
                    {
                        worker.ReportProgress((2 * progressvalue++), "Account confirmed.");
                        // tblaccts.Password = passwd;
                        // tblaccts.Save();
                        return true;
                    }
                    else return false;
                }
            }
        }
        private bool PostTheQuestion(ref Topicsgeneric tbltopics_local, Emailaccounts tblaccts_local, string passwd_local, string proxy_local, string iCode_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Posting a question.");
            tbltopics_local.Link = stdfunc.PostQuestion(tblaccts_local.Username, tblaccts_local.Email, passwd_local, tbltopics_local.Topic, proxy_local, iCode_local);
            tbltopics_local.Save();
            string returnvar = tbltopics_local.Link;
            if (returnvar != "" && returnvar != null && returnvar != "#EANF#" && returnvar != "NODATA")
                return true;
            else return false;
        }
        private void ProduceLongURL() // SHOULD produce only one long URL, so some parameters will be needed (by ref)
        {
            worker.ReportProgress((2 * progressvalue++), "Producing Long URLs.");           
            string KwdsNoSpace = kwd.Replace(" ", "%20");
            try
            {
                KwdsNoSpace = HttpUtility.UrlEncode(kwd);
            }
            catch
            {
                worker.ReportProgress((2 * progressvalue++), "HttpUtility failed and threw and exception.");
                error.Cancel = true;
            }

            string TodaysDate = DateTime.Now.ToString("yyyy-M-d");
            
            if (tbltopics.LongURL1 == "" || tbltopics.LongURL1 == null)
            {
                // PRODUCE FIRST LongURL

                int affprog1 = tbltopics.AffprogramId1;
                if (affprog1 != 0)
                {
                    tblaff.Where.Id.Value = affprog1;
                    tblaff.Where.Id.Operator = WhereParameter.Operand.Equal;
                    tblaff.Query.Load();
                    // System.Threading.Thread.Sleep((1000 * 1 * 4));
                    if (KwdsNoSpace == "titulinis")
                    {
                        tbltopics.LongURL1 = tblaff.HomePageLink.ToString();
                        if (property.ContainsKey("NewURL"))
                        {
                            tbltopics.LongURL1 = "http://partner.jukoshop.com/?source=" + proj.ProjectName.ToLower() + "&date=" + TodaysDate;
                        }
                    }
                    else
                    {
                        tbltopics.LongURL1 = tblaff.PreKeywordLinkPart + KwdsNoSpace + tblaff.PostKeywordLinkPart;
                        if (property.ContainsKey("NewURL"))
                        {
                            tbltopics.LongURL1 = "http://partner.jukoshop.com/?source=" + proj.ProjectName.ToLower() + "&date=" + TodaysDate + "&keyword=" + KwdsNoSpace;
                        }
                    }
                    try
                    {
                           // tbltopics.Save();
                    }
                    catch (DBConcurrencyException ex)
                    {
                        #region Concurrency violations
                        string customErrorMessage;
                        customErrorMessage = "Concurrency violation\n";
                        customErrorMessage += ex.Row[0].ToString() + "\n";
                        customErrorMessage += tbltopics.Query.LastQuery;
                        customErrorMessage += "Last Load() query: \n";
                        MessageBox.Show(customErrorMessage);
                        // Add business logic code to resolve the concurrency violation...
                        #endregion
                    }   
                }

                if (tbltopics.LongURL1 == "" || tbltopics.LongURL1 == null)
                {
                    error.Cancel = true;
                }
            }

            if (tbltopics.LongURL2 == "" || tbltopics.LongURL2 == null)
            {
                // PRODUCE 2nd LongURL
                tblaff.Where.WhereClauseReset();
                int affprog2 = tbltopics.AffprogramId2;
                if (affprog2 != 0)
                {
                    tblaff.Where.Id.Value = affprog2;
                    tblaff.Where.Id.Operator = WhereParameter.Operand.Equal;
                    tblaff.Query.Load();
                    if (KwdsNoSpace == "titulinis")
                    {
                        tbltopics.LongURL2 = tblaff.HomePageLink.ToString();
                    }
                    else tbltopics.LongURL2 = tblaff.PreKeywordLinkPart + KwdsNoSpace + tblaff.PostKeywordLinkPart;
                    // System.Threading.Thread.Sleep((1000 * 1 * 4));
                    // tbltopics.Save();
                }
            }

            try
            {
                tbltopics.Save();
            }
            catch
            {
            }


            worker.ReportProgress((2 * progressvalue++), "Long URLs done.");
        }
        private void ProduceShortURL()  /// should produce only 1 short URL - rewrite this
        {
            worker.ReportProgress((2 * progressvalue++), "Producing Short URLs.");
            TinyURL TinyClass = new TinyURL();

            if ((tbltopics.LongURL1 != "") && (tbltopics.LongURL1 != null))
            {
                int i = 1;
                
                while ((tbltopics.ShortURL1 == "#EANF#" || tbltopics.ShortURL1 == "" || tbltopics.ShortURL1 == "NODATA" || tbltopics.ShortURL1 == null) && (i <= 4)) // Let's not do conversion if the ShortURL1 is None.
                    {
                        tbltopics.ShortURL1 = TinyClass.EURLConvert(tbltopics.LongURL1);
                        tbltopics.Save();
                        i++;
                    }
                
            }

            if ((tbltopics.LongURL2 != "") && (tbltopics.LongURL2 != null))
            {
                int j = 1;
                while ((tbltopics.ShortURL2 == "#EANF#" || tbltopics.ShortURL2 == "" || tbltopics.ShortURL2 == "NODATA" || tbltopics.ShortURL2 == null) && (j <= 4)) // Let's not do conversion if the ShortURL2 is None.
                    {
                        tbltopics.ShortURL2 = TinyClass.EURLConvert(tbltopics.LongURL2);
                        tbltopics.Save();
                        j++;
                    }
                
            }

            if (tbltopics.ShortURL1 != "" && tbltopics.ShortURL1 != null)
            {
                worker.ReportProgress((2 * progressvalue++), "Short URLs done.");
                return;
            }
            else
            {
                worker.ReportProgress((2 * progressvalue++), "Short URLs not produced.");
                error.Cancel = true;
                return;
            }
        }
        private void FilterOutTheNextAnswerTemplate() ///  1 URL ?  2 URLs??  Need to pass these as parameters
        {
            if (tbltopics.CustResponse == "" || tbltopics.CustResponse == null)
            {
                worker.ReportProgress((2 * progressvalue++), "Selecting an answer tmpl.");
                int counter5 = 0;

            SelectAnswersAgain:
                nexttmpl = proj.LastTemplateUsed + 1; // Filter out the next answer template
                // tblresp.Where.LanguageId.Value = proj.Language;
                // tblresp.Where.LanguageId.Operator = WhereParameter.Operand.Equal;

                tblresp.Where.ResponseGroup.Value = AnswerTemplate;
                tblresp.Where.ResponseGroup.Operator = WhereParameter.Operand.Equal;
                try
                {
                    tblresp.Query.Load();
                }
                catch
                {
                    counter5++;
                    if (counter5 >= 4) return;
                    goto SelectAnswersAgain;
                }

                if (nexttmpl > tblresp.RowCount)
                {
                    nexttmpl = 1;
                }

                tblresp.Where.RespGrSpecSequence.Value = nexttmpl;
                tblresp.Where.RespGrSpecSequence.Operator = WhereParameter.Operand.Equal;
                try
                {
                    tblresp.Query.Load();
                }
                catch
                {
                    counter5++;
                    if (counter5 >= 4) return;
                    goto SelectAnswersAgain;
                }

                tbltopics.Response = tblresp.Response;
                try
                {
                    tbltopics.Save();
                }
                catch
                {
                    worker.ReportProgress((2 * progressvalue++), "Ans. couldn't be saved in tbltopics.");
                    error.Cancel = true;
                    return;
                }
            }
            else
            {
                tbltopics.Response = tbltopics.CustResponse;
                try
                {
                    tbltopics.Save();
                }
                catch
                {
                }
            }
            worker.ReportProgress((2 * progressvalue++), "Answer tmpl. selected");

        }
        private string ExceptBlanks(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        continue;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
        private void ReplaceURLs()
        {
            // Replace [URL] in a template
            worker.ReportProgress((2 * progressvalue++), "Replacing [url]s.");

            string beforeurl = "";
            string afterurl = "";
            if (property.ContainsKey("beforeurl"))
                beforeurl = property["beforeurl"];
            if (property.ContainsKey("afterurl"))
                afterurl = property["afterurl"];

            GeneratedResponse = tbltopics.Response;

            if (GeneratedResponse.Contains("[url]"))
                if (tbltopics.ShortURL1 != "" && tbltopics.ShortURL1 != null)
                    GeneratedResponse = GeneratedResponse.Replace("[url]", beforeurl + tbltopics.ShortURL1 + afterurl);
                else
                {
                    error.Cancel = true;
                    return;
                }
            if (GeneratedResponse.Contains("[url1]") && GeneratedResponse.Contains("[url2]"))
            {
                if (tbltopics.ShortURL1 != "" && tbltopics.ShortURL1 != null && tbltopics.ShortURL2 != "" && tbltopics.ShortURL2 != null)
                {
                    GeneratedResponse = GeneratedResponse.Replace("[url1]", beforeurl + tbltopics.ShortURL1 + afterurl);
                    GeneratedResponse = GeneratedResponse.Replace("[url2]", beforeurl + tbltopics.ShortURL2 + afterurl);
                }
                else
                {
                    error.Cancel = true;
                    return;
                }
            }
            
            // Remove white space at the end
            if (GeneratedResponse.EndsWith(" ") || GeneratedResponse.EndsWith("\t"))
                GeneratedResponse = GeneratedResponse.Remove(GeneratedResponse.Length - 1);
            
            // Generate a new line at the end. Some forums like it. 
            GeneratedResponse = GeneratedResponse + Environment.NewLine;

            tbltopics.Response = GeneratedResponse;
            try
            {
                tbltopics.Save();
            }
            catch
            {
                error.Cancel = true;
                return;
            }
            worker.ReportProgress((2 * progressvalue++), "[url]s DONE.");
        }
        private iMacrosPostReturnVars PostTheAnswer()
        {
            worker.ReportProgress((2 * progressvalue++), "Posting an answer.");
            return stdfunc.LoginAndPost(tblaccts.Username, tblaccts.Email, passwd, tblproxies.Proxy, tbltopics.Link, GeneratedResponse, proj.ImacrosLoginPost);
        }
        private void IncrementDBValues(ref Projects proj_local, Dictionary<string, int> UpdateArray, string iMacrosCreateAccountCode_local, string iMacrosLoginAndPostCode_local)
        {
            worker.ReportProgress((2 * progressvalue++), "Incrementing DB values.");
            int counter7 = 0;
            IncrementValuesAgain:
            
                if (iMacrosCreateAccountCode != "" && iMacrosLoginAndPostCode != "")
                {
                    proj_local.ImacrosCreateAccCode = iMacrosCreateAccountCode_local;
                    proj_local.ImacrosLoginPost = iMacrosLoginAndPostCode_local;
                }
                
                if (UpdateArray.ContainsKey("nextemail")) proj_local.LastAccountUsed = UpdateArray["nextemail"];
                if (UpdateArray.ContainsKey("nextproxy")) proj_local.LastProxyUsed = UpdateArray["nextproxy"];
                if (UpdateArray.ContainsKey("nexttmpl")) proj_local.LastTemplateUsed = UpdateArray["nexttmpl"];
                if (UpdateArray.ContainsKey("nexttopic")) proj_local.LastTopicUsed = UpdateArray["nexttopic"];
                
                try
                {
                    proj_local.Save();
                }
                catch
                {
                    counter7++;
                    if (counter7 >= 4)
                    {
                        error.Cancel = true;
                        return;
                    }
                    goto IncrementValuesAgain;
                }
        }
        private void UpdatePostedStatus(sbyte status, string returnURL = "")
        {
            int counter7 = 0;
        
            UpdatePostedStatusAgain:
                tbltopics.PostedStatus = status;
                tbltopics.PostingTime = DateTime.Now;
                tbltopics.PostURLexact = returnURL;
                try
                {
                    tbltopics.Save();
                }
                catch
                {
                    counter7++;
                    if (counter7 >= 4)
                    {
                        error.Cancel = true;
                        return;
                    }
                    goto UpdatePostedStatusAgain;
                }
                worker.ReportProgress((2 * progressvalue++), "Incrementing done.");
        } // Writes: PostedStatus = 1 AND Timeposted = DateTime.Now
        private void ResetAllFilters()
        {
            worker.ReportProgress((2 * progressvalue++), "Resetting filters.");
            try
            {
                tbltopics.Where.WhereClauseReset();
                proj.Where.WhereClauseReset();
                tblaccts.Where.WhereClauseReset();
                tblproxies.Where.WhereClauseReset();
                tblaff.Where.WhereClauseReset();
                tblresp.Where.WhereClauseReset();

                tbltopics.Filter = null;
                proj.Filter = null;
                tblaccts.Filter = null;
                tblproxies.Filter = null;
                tblaff.Filter = null;
                tblresp.Filter = null;
            }
            catch
            {
            }
            worker.ReportProgress((2 * progressvalue++), "Filters reset.");
        }
        private void PauseBetweenPosts(int pause)
        {
            PauseBtwPosts = pause;
            worker.ReportProgress((100), "Pausing for " + PauseBtwPosts.ToString() + " min.");
            for (int j = 1; j <= PauseBtwPosts; j++)
            {
                int Remaining = (PauseBtwPosts - j + 1);
                worker.ReportProgress((100), Remaining.ToString() + " min. remaining.");
                System.Threading.Thread.Sleep((1000 * 60 * 1));  // WAIT BETWEEN POSTS

                if ((worker.CancellationPending == true))  // if the STOP button has been clicked
                {
                    error.Cancel = true;
                    break;
                }
            } // end for
        }

        public void RunSimplePoster(int pausebfconfirm, int pausebfnextpost)
        {
            //Business logic

            while (RunningStatus)
            {
                progressvalue = 2;

                // EmailKwdStatus(false);
                System.Threading.Thread.Sleep((1000 * 5 * 1)); // Sleep 5 seconds (just in case) so that we can manage to Cancel the BW.
               
                        if (CancellationIsPending()) return;
                if (NumberOfKwdsRemaining() == 6) // The threshold is 6 because in the WebApp, the keywords are selected in batches of 5
                {
                    try
                    {
                        EmailKwdStatus(true);
                    }
                    catch
                    { 
                    }
                }
                        if (CancellationIsPending()) return;                
                SleepIfNighttime();
                        if (CancellationIsPending()) return;
                SelectProject(ref proj, ProjectName);
                    if (CancellationIsPending()) return;
                SaveMacroCodes();
                        if (CancellationIsPending()) return;
                SelectNextTopic();
                UpdatePostedStatus(2);
                        if (CancellationIsPending()) return;
                WaitForKeywordsOrShutTheProcessDown();
                        if (CancellationIsPending()) return;
                nextemail = SelectNextEmail(ref tblaccts,proj.LastAccountUsed);
                        if (CancellationIsPending()) return;
                nextproxy = SelectNextProxy(ref tblproxies, proj.LastProxyUsed);
                        if (CancellationIsPending()) return;
                CheckIfProxyIsWorkingSelectNextIfNotWorking();
                        if (CancellationIsPending()) return;

                if (CreateNewAccount(tblaccts, tblproxies.Proxy, proj.ImacrosCreateAccCode))
                {
                    if (CancellationIsPending()) return;

                    WaitSomeTime(pausebfconfirm); //  wait 2 minutes

                    if (CancellationIsPending()) return;

                    #region AskCom situation where we need to post the question first
                    if (property.ContainsKey("postQnA"))
                    {
                        try
                        {
                            int pQnA = Convert.ToInt32(property["postQnA"]);
                            if (pQnA == 1) postQnA = true;
                        }
                        catch { }
                    }

                    if (postQnA)
                    {
                        questionposted = PostTheQuestion(ref tbltopics, tblaccts, passwd, tblproxies.Proxy, proj.ImacrosCodeGeneric);
                        
                        if (questionposted)
                        {
                            /*
                             * increment Projects table (account, proxy)
                             * select new account from AccountsTable
                             * select new proxy 
                             */
                            Dictionary<string, int> temp_dict = new Dictionary<string, int>();
                            temp_dict.Add("nextemail", nextemail);
                            temp_dict.Add("nextproxy", nextproxy);
                            IncrementDBValues(ref proj, temp_dict, iMacrosCreateAccountCode, iMacrosLoginAndPostCode);
                            temp_dict = null;
                            SelectProject(ref proj, ProjectName);
                            if (CancellationIsPending()) return;
                            nextemail = SelectNextEmail(ref tblaccts, proj.LastAccountUsed);
                            if (CancellationIsPending()) return;
                            nextproxy = SelectNextProxy(ref tblproxies, proj.LastProxyUsed);
                            if (CancellationIsPending()) return;
                            CheckIfProxyIsWorkingSelectNextIfNotWorking();
                            if (CancellationIsPending()) return;
                            if (!CreateNewAccount(tblaccts, tblproxies.Proxy, proj.ImacrosCreateAccCode))
                                questionposted = false;
                        }
                    }
                    #endregion

                    confirmed = ConfirmNewAccount();
                    if (confirmed && questionposted)
                    {
                        UpdatePostedStatus(3);
                        ProduceLongURL();
                        if (CancellationIsPending()) return;
                        ProduceShortURL();
                        if (CancellationIsPending()) return;
                        FilterOutTheNextAnswerTemplate();
                        if (CancellationIsPending()) return;
                        ReplaceURLs();
                        if (CancellationIsPending()) return;

                        if (property.ContainsKey("password") && property.ContainsKey("shiftby"))
                        {
                            // if there should be another email with the password
                            confirmed = ConfirmNewAccount(property["password"], Convert.ToInt32(property["shiftby"]));
                        }
                    }

                    if (confirmed && questionposted) // if still confirmed (after (optionally) getting a password from the email 
                    {
                        poster = new iMacrosPostReturnVars();
                        poster = PostTheAnswer();

                        if (CancellationIsPending()) return;
                        
                        if (poster.getSuccess())
                        {
                            UpdatePostedStatus(1, poster.getReturnURL());
                            Dictionary<string,int> temp_dict = new Dictionary<string,int>();
                            temp_dict.Add("nextemail", nextemail);
                            temp_dict.Add("nextproxy", nextproxy);
                            temp_dict.Add("nexttmpl", nexttmpl);
                            temp_dict.Add("nexttopic", nexttopic);
                            IncrementDBValues(ref proj, temp_dict, iMacrosCreateAccountCode, iMacrosLoginAndPostCode);
                            temp_dict = null;
                            if (CancellationIsPending()) return;
                        }
                        else // if not successfully posted (i.e. no TinyURL in the final page result)
                        {
                            UpdatePostedStatus(4, poster.getReturnURL());
                            Dictionary<string, int> temp_dict = new Dictionary<string, int>();
                            temp_dict.Add("nextemail", nextemail);
                            temp_dict.Add("nextproxy", nextproxy);
                            temp_dict.Add("nexttmpl", nexttmpl);
                            temp_dict.Add("nexttopic", nexttopic);
                            IncrementDBValues(ref proj, temp_dict, iMacrosCreateAccountCode, iMacrosLoginAndPostCode);
                            temp_dict = null;
                            if (CancellationIsPending()) return;
                        }
                    }
                    else  // if account not confirmed, switch to a new account and proxy:
                    {
                        Dictionary<string, int> temp_dict = new Dictionary<string, int>();
                        temp_dict.Add("nextemail", nextemail);
                        temp_dict.Add("nextproxy", nextproxy);
                        temp_dict.Add("nexttmpl", nexttmpl);
                        temp_dict.Add("nexttopic", nexttopic);
                        IncrementDBValues(ref proj, temp_dict, iMacrosCreateAccountCode, iMacrosLoginAndPostCode);
                        temp_dict = null;
                        if (CancellationIsPending()) return;
                    }
                    
                    if (CancellationIsPending()) return;

                    PauseBetweenPosts(pausebfnextpost); // only if the Account was created, then pause, otherwise LOOP again

                    if (CancellationIsPending()) return;

                } // end CreateNewAccount
                ResetAllFilters();

                if (CancellationIsPending()) return;

            } // end while

        } // end simple poster

    } // END CLASS
} // END NAMESPACE
