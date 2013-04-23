using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using MyGeneration.dOOdads;


namespace iMacrosPostingDashboard
{
    class StdPosterFuctions
    {
        // Define standard SourceProgram types and methods -> They are already pretty well defined by the dOOdads model. 
        // And the specific *.cs file instantiates the object
        

        // LoadMainVariables - incl. Which Source (Forum) we are using
        
        // Check if the keyword has been set for the next topic

        private static iMacros.App m_app;
        private static int m_timeout = 600;
        private static int close_timeout = 3;

        public static string NextKeyword(Projects proj, Topicsgutefrage tbltopics, string ProjectName)
        {
            // proj = new Projects();
            // tbltopics = new Topicsgutefrage();
            
            // DEPRECIATED
            return null;
        }

        public static bool CreateAccount(string username, string email, string pwd, string fname, string proxy, string CreateMacro)
        {
            // READ iMacros Code
            // SET variables
            // Execute the code
            // Return success / failure --> Save to DB??

            bool Success = false;

            
            
            // tblaccts.Filter = Projects.ColumnNames.Id + " = " + nextemail;
            if (email != "")
            {

                m_app = new iMacros.App();
                iMacros.Status s;

                s = m_app.iimOpen("", true, m_timeout);
                s = m_app.iimSet("Username", username);
                s = m_app.iimSet("Email", email);
                s = m_app.iimSet("Password", pwd);
                s = m_app.iimSet("FirstName", fname);
                s = m_app.iimSet("Proxy", proxy);

                // Execute macro
                s = m_app.iimPlayCode(CreateMacro, m_timeout);
                // Success = m_app.iimGetExtract(1);
                try
                {
                    s = m_app.iimClose(close_timeout);
                }
                catch
                {
                    // 
                    goto JustContinue;
                }

            JustContinue:

                m_app = null;

                // Get variable from macro and return
            }


            // *************************************************
            Success = true; // TEMPORARY VALUE for DEBUGGING 
            // *************************************************

            return Success;
        }

        public static bool CreateAccountPurseBlog(string username, string email, string pwd, string fname, string proxy, string CreateMacro)
        {

            // STANDARD VARIABLES
            String result = "";
            int errors = 0;

            string ConfirmationTxt = "";
            int inc = 0;
            bool Success = false;
            if (email != "")
            {

                m_app = new iMacros.App();
                iMacros.Status s;
            
            TryAgain:
                try
                {
                    s = m_app.iimOpen("", true, m_timeout);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "open " + Convert.ToString(s) + "; ";

                    s = m_app.iimSet("Username", username);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "set " + Convert.ToString(s) + "; ";

                    s = m_app.iimSet("Email", email);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "set " + Convert.ToString(s) + "; ";

                    s = m_app.iimSet("Password", pwd);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "set " + Convert.ToString(s) + "; ";

                    s = m_app.iimSet("FirstName", fname);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "set " + Convert.ToString(s) + "; ";

                    s = m_app.iimSet("Proxy", proxy);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "set " + Convert.ToString(s) + "; ";

                    // Execute macro
                    s = m_app.iimPlayCode(CreateMacro, m_timeout);
                    if (s != iMacros.Status.sOk) errors++;
                    result = result + "play " + Convert.ToString(s) + "; ";

                    ConfirmationTxt = m_app.iimGetExtract(1);

                }
                catch
                {
                    MessageBox.Show("Result: " + result);
                    goto TryAgain;
                }
                
                string[] testsplit = ConfirmationTxt.Split(' ');
                
                if (ConfirmationTxt != "" && ConfirmationTxt != "NODATA" && ConfirmationTxt != "#EANF#" && testsplit.Count() >= 7)
                {
                    string FinishCreateMacro = "";
                
                    FinishCreateMacro = FinishCreateMacro + "WAIT SECONDS=3" + "\n";
                    FinishCreateMacro = FinishCreateMacro + "TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:register ATTR=NAME:humanverify[input] CONTENT={{Solution}}" + "\n";
                    FinishCreateMacro = FinishCreateMacro + "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:register ATTR=NAME:options[adminemail] CONTENT=NO" + "\n";
                    FinishCreateMacro = FinishCreateMacro + "WAIT SECONDS=3" + "\n";
                    FinishCreateMacro = FinishCreateMacro + "TAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:register ATTR=CLASS:button" + "\n";
                    FinishCreateMacro = FinishCreateMacro + "WAIT SECONDS=3" + "\n";

                    s = m_app.iimSet("Solution", SolveCaptchaPurseBlog(ConfirmationTxt));
                    s = m_app.iimPlayCode(FinishCreateMacro, m_timeout);

                    Success = true; // TEMPORARY VALUE for DEBUGGING 
                
                }
                else
                {
                    inc++;
                    if (inc > 5)
                    {
                        Success = false; 
                        goto CloseApp;
                    }
                    s = m_app.iimClose();
                    m_app = null;
                    goto TryAgain;
                }

            CloseApp:
                try
                {
                    s = m_app.iimClose(close_timeout);
                }
                catch
                {
                    // 
                    goto JustContinue;
                }

            JustContinue:

                m_app = null;

                // Get variable from macro and return
            }


            // *************************************************
            // *************************************************

            return Success;
        }

        public static string[] CreateAccountAskCom(string username, string email, string pwd, string fname, string proxy, string CreateMacro, string topic)
        {
            // READ iMacros Code
            // SET variables
            // Execute the code
            // Return success / failure --> Save to DB??

            //bool Success = false;

            string[] ErrAndId = new string[2];

            // tblaccts.Filter = Projects.ColumnNames.Id + " = " + nextemail;
            if (topic != "")
            {

            TryAgain:
                m_app = new iMacros.App();
                iMacros.Status s;

                s = m_app.iimOpen("", true, m_timeout);
                s = m_app.iimSet("Username", username);
                s = m_app.iimSet("Email", email);
                s = m_app.iimSet("Password", pwd);
                s = m_app.iimSet("FirstName", fname);
                s = m_app.iimSet("Proxy", proxy);
                s = m_app.iimSet("Topic", topic);
                
                // Execute macro
                s = m_app.iimPlayCode(CreateMacro, m_timeout);
                try
                {
                    ErrAndId[1] = m_app.iimGetExtract(1);
                    ErrAndId[0] = m_app.iimGetExtract(2);
                }
                catch
                {
                    //
                    goto TryAgain;
                }

                // Success = m_app.iimGetExtract(1);
                try
                {
                    s = m_app.iimClose(close_timeout);
                }
                catch
                {
                    // 
                    goto JustContinue;
                }

            JustContinue:

                m_app = null;

                // Get variable from macro and return
            }


            // *************************************************
            //Success = true; // TEMPORARY VALUE for DEBUGGING 
            // *************************************************

            return ErrAndId;
        }

        public static void ReportBadCaptcha(string CaptchaId)
        {
            String url = "http://api.dbcapi.me/api/captcha/" + CaptchaId + "/report";
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=Jukas";
            postData += "&password=jukass";
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                string DebugResp = responseString; // For DEBUGing only. To be deleted.
                
            }
            catch
            {
            }

        }

        public static string PostQuestionAskCom(string Email, string Password, string Topic, string Proxy, string Macro)
        {
                        // GET iMacros code
            // SET Variables
            // Execute the code
            // Return Success / Failure
            //bool Success = false;

            string QuestionLink = "";
            
            m_app = new iMacros.App();
            iMacros.Status s;

            s = m_app.iimOpen("", true, 5);

            s = m_app.iimSet("Email", Email);
            s = m_app.iimSet("Password", Password);
            s = m_app.iimSet("Proxy", Proxy);
            s = m_app.iimSet("Topic", Topic);

            // Execute macro
            s = m_app.iimPlayCode(Macro, m_timeout);

            try
            {
                QuestionLink = m_app.iimGetExtract(1);
            }
            catch
            {
                goto Continue1;
            }

        Continue1:

            try
            {
                s = m_app.iimClose(close_timeout);
            }
            catch
            {
                // 
                goto JustContinue;
            }

        JustContinue:

            m_app = null;

            // Get variable from macro and return
            
            
        // *************************************************
        //Success = true; // TEMPORARY VALUE for DEBUGGING 
        // *************************************************

            return QuestionLink;
             
        }

        public static bool ConfirmAccount(string email, string pwd, string lnkstructure, string sdremail, string proxy, int ShiftLinkBy)
        {
            // GET iMacros code (or use a static code)
            // SET Variables
            // Execute the code
            // Return Success / Failure

            string LinkExtracted = HotmailPOP.ExtractConfLink(email, pwd, lnkstructure, sdremail, ShiftLinkBy);
            bool Success = false;
            
            if (LinkExtracted != null)
            {

                // Static Code for all programs
                string macro = "Version Build = 8032216" + "\r\n";
                macro = macro + "TAB T=1" + "\r\n";
                macro = macro + "TAB CLOSEALLOTHERS" + "\r\n";
                macro = macro + "URL GOTO=about:blank" + "\r\n";
                macro = macro + "WAIT SECONDS=3" + "\r\n";
                macro = macro + "PROXY ADDRESS={{Proxy}}" + "\r\n";
                // macro = macro + "ONLOGIN USER=Jukas PASSWORD=7qUzTq7V" + "\r\n";
                macro = macro + "URL GOTO={{ConfLink}}" + "\r\n";
                macro = macro + "WAIT SECONDS=3" + "\r\n";

                m_app = new iMacros.App();
                iMacros.Status s;

                s = m_app.iimOpen("", true, 5);
                s = m_app.iimSet("ConfLink", LinkExtracted);
                s = m_app.iimSet("Email", email);
                s = m_app.iimSet("Password", pwd);
                s = m_app.iimSet("Proxy", proxy);

                // Execute macro
                s = m_app.iimPlayCode(macro, m_timeout);
                // Success = m_app.iimGetExtract(1);
                try
                {
                    s = m_app.iimClose(close_timeout);
                }
                catch
                {
                    // 
                    goto JustContinue;
                }

            JustContinue:

                m_app = null;

                // Get variable from macro and return

                // *************************************************
                Success = true; // TEMPORARY VALUE for DEBUGGING 
                // *************************************************
            }

            return Success;
        }

        public static void UnitTest()
        {
            //
            Responses tblresp = new Responses();
            Topicsgutefrage tbltopics = new Topicsgutefrage();

            tblresp.Where.Id.Value = 1;
            tblresp.Where.Id.Operator = WhereParameter.Operand.Equal;
            tblresp.Query.Load();

            // Replace [URL] in a template

            string cresponse = tblresp.Response;
            cresponse = cresponse.Replace("[url]", "http://tinyurl.com/");

            tbltopics.Where.Id.Value = 5270;
            tbltopics.Where.Id.Operator = WhereParameter.Operand.Equal;
            tbltopics.Query.Load();

            tbltopics.Response = cresponse;
            tbltopics.Save();
            string query = tbltopics.Query.LastQuery;
            // MessageBox.Show(query);
        }

        public static bool LoginAndPost(string Username, string Email, string Password, string Proxy, string LinkToPost, string GeneratedResponse, string macro)
        {
            // GET iMacros code
            // SET Variables
            // Execute the code
            // Return Success / Failure
            bool Success = false;

            // Generate answer:

            // Post the Response iMacros

                m_app = new iMacros.App();
                iMacros.Status s;

                s = m_app.iimOpen("", true, 5);
                s = m_app.iimSet("Username", Username);
                s = m_app.iimSet("Email", Email);
                s = m_app.iimSet("Password", Password);
                s = m_app.iimSet("Proxy", Proxy);
                s = m_app.iimSet("LinkToPost", LinkToPost);
                s = m_app.iimSet("Answer", GeneratedResponse);

                // Execute macro
                s = m_app.iimPlayCode(macro, m_timeout);
                // Success = m_app.iimGetExtract(1);

                try
                {
                    s = m_app.iimClose(close_timeout);
                }
                catch
                {
                    // 
                    goto JustContinue;
                }

            JustContinue:
                m_app = null;

                // Get variable from macro and return
            
            
            // *************************************************
            Success = true; // TEMPORARY VALUE for DEBUGGING 
            // *************************************************
          
            
            return Success;
        }

        private static string SolveCaptchaPurseBlog(string VerificationWord)
        {
            //
            // string Str= "one all of the rest";

            string[] words = VerificationWord.Split(' ');

            switch (words[7])
            {
                case "first":
                case "1.":
                    {
                        // You can use the parentheses in a case body.
                        return "pumps";
                    }
                case "second":
                case "2.":
                    {
                        // You can use the parentheses in a case body.
                        return "sunset";
                    }
                case "third":
                case "3.":
                    {
                        // You can use the parentheses in a case body.
                        return "elevator";
                    }
                case "fourth":
                case "4.":
                    {
                        // You can use the parentheses in a case body.
                        return "stream";
                    }
                default:
                    // You can use the default case.
                    return "";
            }
        }

    }

}


