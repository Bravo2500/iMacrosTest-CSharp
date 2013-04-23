using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using MyGeneration.dOOdads;
using System.Web;

namespace iMacrosPostingDashboard
{
    class GuteFrageDE
    {
        // private static string senderemail;

        public static int RunningStatus = 0;

        #region TESTING CODE
        public static void UnitTest()
        {
            //

        }


        public static void PostTest(Object sender, DoWorkEventArgs e)
        {
            //
            BackgroundWorker worker = sender as BackgroundWorker;
            int delay = 1000; // 1 second
            int i = 1;

            while (!worker.CancellationPending)
            {
                System.Threading.Thread.Sleep(delay);
                worker.ReportProgress((i * 10));
                i++;
                if (i > 10) i = 1;
            }
            e.Cancel = true;
        }
        #endregion

        public static DateTime StartTime = Convert.ToDateTime("09:03:00 AM");
        public static DateTime EndTime = Convert.ToDateTime("10:00:00 PM");


        public static void PostNextTopic(Object sender, DoWorkEventArgs e)
        {
            #region CommentTemplate
            // ************* TEMPLATE FOR ALL OTHER POSTERS AS WELL *************

            // Load Variables:  LastTopic, LastEmail, Proxy, Template, LinkPattern, SenderEmail, iMacrosScripts: AccCreator, Poster, etc.
            // Get last GuteFrage topic 
            // Check if it has a keyword 
            // Get last used email account 
            // Each subsequent process is conditional on the previous one completing correctly         
            // Create a GuteFrage Account (iMacros)
            // Extract confirmation link (HotmailPOP)

            // Confirm Account (iMacros)

            // Some intermediary steps:
            // Get short URL (check if it exists, if not, create it from LongURL1)
            // Replace [URL] in a template
            // Post the answer to GuteFrage (iMacros)

            // *******************************************************************

            // Get last GuteFrage topic id, email id, proxy id, answer_tmpl id 
            #endregion

            #region Set ALL Main Variables

            // INITialize all DB objects
            BackgroundWorker worker = sender as BackgroundWorker;
            Projects proj = new Projects();
            Emailaccounts tblaccts = new Emailaccounts();
            Proxies tblproxies = new Proxies();
            Responses tblresp = new Responses();
            Affiliateprograms tblaff = new Affiliateprograms();
            int nexttmpl = 0;
            int nextproxy = 0;

            // Project Specific Variables
            Topicsgutefrage tbltopics = new Topicsgutefrage();
            string ProjectName = "GuteFrageDE";
            int PauseBtwPosts = 30; // 30 minutes
            
            #endregion


            while (RunningStatus != 1) // while there are keywords
            {
                if ((worker.CancellationPending == true))  // if the STOP button has been clicked
                {
                    e.Cancel = true;
                    break;
                }
                else  // If not STOPed, then continue with the next keyword
                {

                    #region SLEEP IF CURRENT TIME OUT OF DAYTIME POSTING TIME

                    if ((DateTime.Now.TimeOfDay < StartTime.TimeOfDay) || (DateTime.Now.TimeOfDay > EndTime.TimeOfDay)) worker.ReportProgress((2 * 0 * 10), "Night time sleeping until: " + StartTime.ToShortTimeString());

                    while ((DateTime.Now.TimeOfDay < StartTime.TimeOfDay) || (DateTime.Now.TimeOfDay > EndTime.TimeOfDay))
                    {
                        //
                        System.Threading.Thread.Sleep((1000 * 60 * 10));
                    }

                    #endregion


                    worker.ReportProgress((2 * 1 * 10), "Initialized. Selecting the next topic. ");

                    #region SELECT THE PROJECT AND SELECT NEXT TOPIC

                    string kwd = "";
                    int nexttopic = 0;

                    proj.Where.ProjectName.Value = ProjectName;
                    proj.Where.ProjectName.Operator = WhereParameter.Operand.Equal;
                    proj.Query.Load();

                    nexttopic = proj.LastTopicUsed + 1;

                    tbltopics.Where.Id.Value = nexttopic;
                    tbltopics.Where.Id.Operator = WhereParameter.Operand.Equal;
                    tbltopics.Query.Load();
                    kwd = tbltopics.ProductKeyword;
                    
                    #endregion

                    if (kwd == "")
                    {
                        #region WAIT FOR KEYWORDS OR SHUT THE PROCESS DOWN

                        worker.ReportProgress((2 * 2 * 10),"No keyword found for the next topic. Waiting for more keywords.");
                        System.Threading.Thread.Sleep((1000 * 60 * 5));

                        if ((worker.CancellationPending == true))
                        {
                            e.Cancel = true;
                            RunningStatus = 1;
                            break;
                        }
                        #endregion
                    }
                    else
                    {

                        #region SELECT NEXT EMAIL AND NEXT PROXY

                        int nextemail = proj.LastAccountUsed + 1; // Filter the Email Account
                        tblaccts.Where.Id.Value = nextemail;
                        tblaccts.Where.Id.Operator = WhereParameter.Operand.Equal;
                        tblaccts.Query.Load();
                        try
                        {
                            string CurrentEmail = tblaccts.Email;
                        }
                        catch
                        {
                            MessageBox.Show("There are no more emails");
                        }

                        // Filter the ProxyTable

                        nextproxy = proj.LastProxyUsed + 1;
                        tblproxies.LoadAll();
                        int proxycount = tblproxies.RowCount;
                        if (nextproxy > proxycount)
                        {
                            nextproxy = 1;
                        }

                        tblproxies.Where.Id.Value = nextproxy;
                        tblproxies.Where.Id.Operator = WhereParameter.Operand.Equal;
                        tblproxies.Query.Load();
                        #endregion

                        #region CHECK IF PROXY IS WORKING, SELECT NEXT IF NOT WORKING

                        Httpcalls httpcall = new Httpcalls();

                        while (!httpcall.IsAlive(tblproxies.Proxy))
                        {
                            tblproxies.Active = 0;
                            tblproxies.Save();

                            nextproxy = nextproxy + 1;
                            if (nextproxy > proxycount)
                            {
                                nextproxy = 1;
                            }

                            tblproxies.Where.Id.Value = nextproxy;
                            tblproxies.Where.Id.Operator = WhereParameter.Operand.Equal;
                            tblproxies.Query.Load();
                        }

                        tblproxies.Active = 1;
                        tblproxies.Save();

                        #endregion  CHECK IF PROXY IS WORKING, SELECT NEXT IF NOT WORKING

                        StdPosterFunctionsNonStatic stdfunc = new StdPosterFunctionsNonStatic();

                        if (stdfunc.CreateAccount(tblaccts.Username, tblaccts.Email, tblaccts.Password, tblaccts.FirstName, tblproxies.Proxy, proj.ImacrosCreateAccCode))
                        {
                            #region WAIT SOME TIME
                            worker.ReportProgress((2 * 3 * 10), "Account created. Pausing 1min. before confirming an account...");
                            System.Threading.Thread.Sleep((1000 * 60 * 2));
                            #endregion

                            if ( true  /* stdfunc.ConfirmAccount(tblaccts.Email, tblaccts.Password, proj.Linkstructure, proj.Senderemail, tblproxies.Proxy, proj.ShiftLinkstructureBy)   */  )
                            {

                                worker.ReportProgress((2 * 4 * 10), "Account confirmed. Shortening URLs and Posting...");

                                #region Produce LongURLs

                                string KwdsNoSpace = kwd.Replace(" ", "%20");
                                try
                                {
                                    KwdsNoSpace = HttpUtility.UrlEncode(kwd);
                                }
                                catch
                                {
                                    worker.ReportProgress((2 * 4 * 10), "HttpUtility failed and threw and exception.");
                                    e.Cancel = true;
                                    break;
                                }

                                if (tbltopics.LongURL1 == "" || tbltopics.LongURL1 == null)
                                {
                                    // PRODUCE FIRST LongURL
                                    tblaff.Where.Id.Value = tbltopics.AffprogramId;
                                    tblaff.Where.Id.Operator = WhereParameter.Operand.Equal;
                                    tblaff.Query.Load();
                                    tbltopics.LongURL1 = tblaff.PreKeywordLinkPart + KwdsNoSpace + tblaff.PostKeywordLinkPart;
                                    tbltopics.Save();
                                }

                             /*   if (tbltopics.LongURL2 == "" || tbltopics.LongURL2 == null)
                                {
                                    // PRODUCE 2nd LongURL
                                    tblaff.Where.WhereClauseReset();
                                    tblaff.Where.Id.Value = tbltopics.AffprogramId2;
                                    tblaff.Where.Id.Operator = WhereParameter.Operand.Equal;
                                    tblaff.Query.Load();
                                    tbltopics.LongURL2 = tblaff.PreKeywordLinkPart + KwdsNoSpace + tblaff.PostKeywordLinkPart;
                                    tbltopics.Save();
                                }
                            */
                                #endregion
                            
                                #region Produce ShortURLs

                                TinyURL TinyClass = new TinyURL();

                                for (int i = 1; i <= 4; i++)
                                {
                                    if (tbltopics.ShortURL1 == "#EANF#" || tbltopics.ShortURL1 == "" || tbltopics.ShortURL1 == "NODATA" || tbltopics.ShortURL1 == null) // Let's not do conversion if the ShortURL1 is None.
                                    {
                                        tbltopics.ShortURL1 = TinyClass.ImacrosTinyurlConvert(tbltopics.LongURL1);
                                        tbltopics.Save();
                                    }
                                }

                                /*   for (int i = 1; i <= 4; i++)
                                   {
                                       if (tbltopics.ShortURL2 == "#EANF#" || tbltopics.ShortURL2 == "" || tbltopics.ShortURL2 == "NODATA" || tbltopics.ShortURL2 == null) // Let's not do conversion if the ShortURL2 is None.
                                       {
                                           tbltopics.ShortURL2 = TinyURL.ImacrosTinyurlConvert(tbltopics.LongURL2);
                                           tbltopics.Save();
                                       }
                                   }
                               */
                                #endregion

                                #region Filter out the next answer template AND replace [url]s

                                nexttmpl = proj.LastTemplateUsed + 1; // Filter out the next answer template

                                tblresp.Where.LanguageId.Value = proj.Language;
                                tblresp.Where.LanguageId.Operator = WhereParameter.Operand.Equal;

                                tblresp.Where.ResponseGroup.Value = "AmazonDE-one-link";
                                tblresp.Where.ResponseGroup.Operator = WhereParameter.Operand.Equal;

                                tblresp.Query.Load();

                                if (nexttmpl > tblresp.RowCount)
                                {
                                    nexttmpl = 1;
                                }

                                tblresp.Where.RespGrSpecSequence.Value = nexttmpl;
                                tblresp.Where.RespGrSpecSequence.Operator = WhereParameter.Operand.Equal;
                                tblresp.Query.Load();

                                // Replace [URL] in a template

                                string GeneragedResponse = tblresp.Response;
                                GeneragedResponse = GeneragedResponse.Replace("[url]", tbltopics.ShortURL1);
                                // GeneragedResponse = GeneragedResponse.Replace("[url2]", tbltopics.ShortURL2);

                                tbltopics.Response = GeneragedResponse;
                                tbltopics.Save();

                                #endregion

                                #region Post The Answer



                                if (tbltopics.ShortURL1 != "" && tbltopics.ShortURL1 != null)
                                {

                                    if (stdfunc.LoginAndPost(tblaccts.Username, tblaccts.Email, tblaccts.Password, tblproxies.Proxy, tbltopics.Link, GeneragedResponse, proj.ImacrosLoginPost))
                                    {
                                        //
                                        worker.ReportProgress((2 * 5 * 10), "Posted successfully. Updating database, and switching to a new topic...");

                                        proj.LastAccountUsed++;
                                        proj.LastProxyUsed = nextproxy;
                                        proj.LastTemplateUsed = nexttmpl;
                                        proj.LastTopicUsed++;
                                        proj.Save();

                                        tbltopics.PostedStatus = 1;
                                        tbltopics.PostingTime = DateTime.Now;
                                        tbltopics.Save();

                                        // MessageBox.Show("Everything completed well. Switching to the next topic.");
                                    }
                                }
                                #endregion

                            }
                        }
                    }

                    #region Reset all Filters

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

                    #endregion

                    if ((worker.CancellationPending == true))  // if the STOP button has been clicked
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        worker.ReportProgress((2 * 5 * 10), "Pausing for " + PauseBtwPosts.ToString() + " min.");

                        for (int j = 1; j <= PauseBtwPosts; j++)
                        {
                            int Remaining = (PauseBtwPosts - j + 1);
                            worker.ReportProgress((2 * 5 * 10), Remaining.ToString() + " min. remaining.");
                            System.Threading.Thread.Sleep((1000 * 60 * 1));  // WAIT BETWEEN POSTS
                            if ((worker.CancellationPending == true))  // if the STOP button has been clicked
                            {
                                e.Cancel = true;
                                break;
                            }
                        }
                    }
                } // end IF Keyword is ""

            } // end While (RunningStatus != 1) script

        } // end PostNextTopic();

        // *************** THE END of the NEW SCRIPT ******************************
    }
}
