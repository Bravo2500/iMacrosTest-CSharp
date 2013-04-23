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
    public class MumsNetNew
    {

        public static int RunningStatus = 0;

        public static DateTime StartTime = Convert.ToDateTime("10:01:00 AM");
        public static DateTime EndTime = Convert.ToDateTime("10:00:00 PM");

        public MumsNetNew()
        { 
        }

        public static void PostNextTopic(Object sender, DoWorkEventArgs e)
        {
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
            Topicsmumsnet tbltopics = new Topicsmumsnet(); 
            string ProjectName = "MumsNetUK";
            int PauseBtwPosts = 60; // 60 minutes

            #endregion

            #region CHECK if CancellationPending = true

            while (RunningStatus != 1) // while there are keywords
            {
                if ((worker.CancellationPending == true))  // if the STOP button has been clicked
                {
                    e.Cancel = true;
                    break;
                }
                else  // If not STOPed, then continue with the next keyword
                {

                    #endregion CHECK if CancellationPending = true

                    #region SLEEP IF CURRENT TIME OUT OF DAYTIME POSTING TIME

                    if ((DateTime.Now.TimeOfDay < StartTime.TimeOfDay) || (DateTime.Now.TimeOfDay > EndTime.TimeOfDay)) worker.ReportProgress((2 * 0 * 10), "Night time sleeping until: " + StartTime.ToShortTimeString());

                    while ((DateTime.Now.TimeOfDay < StartTime.TimeOfDay) || (DateTime.Now.TimeOfDay > EndTime.TimeOfDay))
                    {
                        //
                        System.Threading.Thread.Sleep((1000 * 60 * 10));
                    }

                    #endregion

                    #region SELECT THE PROJECT AND SELECT NEXT TOPIC

                    worker.ReportProgress((2 * 1 * 10), "Initialized. Selecting the next topic. ");

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


                    worker.ReportProgress((2 * 2 * 10));
                    #endregion

                    if (kwd == "")
                    {
                        #region WAIT FOR KEYWORDS OR SHUT THE PROCESS DOWN
                        worker.ReportProgress((2 * 2 * 10), "No keyword found for the next topic. Waiting for more keywords.");
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
                            worker.ReportProgress((2 * 3 * 10), "Now confirming an account...");

                            if (stdfunc.ConfirmAccount(tblaccts.Email, tblaccts.Password, proj.Linkstructure, proj.Senderemail, tblproxies.Proxy, proj.ShiftLinkstructureBy))
                            {

                                worker.ReportProgress((2 * 4 * 10), "Account confirmed. Shortening URLs and Posting...");

                                #region Produce LongURLs
                                /*
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
                                    tblaff.Where.Id.Value = tbltopics.AffprogramId1;
                                    tblaff.Where.Id.Operator = WhereParameter.Operand.Equal;
                                    tblaff.Query.Load();
                                    tbltopics.LongURL1 = tblaff.PreKeywordLinkPart + KwdsNoSpace + tblaff.PostKeywordLinkPart;
                                    tbltopics.Save();
                                }
                                
                                if (tbltopics.LongURL2 == "" || tbltopics.LongURL2 == null)
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
                                /*
                                for (int i = 1; i <= 4; i++)
                                {
                                    if (tbltopics.ShortURL1 == "#EANF#" || tbltopics.ShortURL1 == "" || tbltopics.ShortURL1 == "NODATA" || tbltopics.ShortURL1 == null) // Let's not do conversion if the ShortURL1 is None.
                                    {
                                        tbltopics.ShortURL1 = TinyURL.ImacrosTinyurlConvert(tbltopics.LongURL1);
                                        tbltopics.Save();
                                    }
                                }
                                

                                for (int i = 1; i <= 4; i++)
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
                                /*
                                nexttmpl = proj.LastTemplateUsed + 1; // Filter out the next answer template
                                
                                tblresp.Where.LanguageId.Value = proj.Language;
                                tblresp.Where.LanguageId.Operator = WhereParameter.Operand.Equal;

                                tblresp.Where.ResponseGroup.Value = "AmazonUK-one-link";
                                tblresp.Where.ResponseGroup.Operator = WhereParameter.Operand.Equal;
                                
                                tblresp.Query.Load();
                                
                                if (nexttmpl > tblresp.RowCount)
                                {
                                    nexttmpl = 1;
                                }
                                
                                
                                tblresp.Where.RespGrSpecSequence.Value = nexttmpl;
                                tblresp.Where.RespGrSpecSequence.Operator = WhereParameter.Operand.Equal;
                                tblresp.Query.Load();
                                */

                                // Replace [URL] in a template

                                string GeneragedResponse = "";
                                // GeneragedResponse = tblresp.Response;

                                GeneragedResponse = tbltopics.CustResponse; // Agnes post'ams TEMP sakinys
                                
                                GeneragedResponse = GeneragedResponse.Replace("[url]", tbltopics.ShortURL1);
                                // GeneragedResponse = GeneragedResponse.Replace("[url]", "[[" + tbltopics.ShortURL2 + " eBay" + "]]");

                                tbltopics.Response = GeneragedResponse;
                                tbltopics.Save();

                                #endregion

                                #region Post The Answer

                                if (tbltopics.ShortURL1 != "" && tbltopics.ShortURL1 != null && GeneragedResponse != "")
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
                        } // end if CreateAccount
                        
                        stdfunc = null;

                    } // end if kwd == ""
                    
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

                    // stdfunc = null;

                    #endregion

                    #region PAUSE BETWEEN POSTS

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
                     #endregion PAUSE BETWEEN POSTS

// *********************************************  COPIED FROM ORIGINAL MUMSNET

                } // end if CancelationPending
            } // end while runningstatus != 1
        } // end PostNextTopic()
    }
}


