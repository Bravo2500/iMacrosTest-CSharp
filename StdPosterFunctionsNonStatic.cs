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
    public class StdPosterFunctionsNonStatic
    {

        int m_timeout = 600;
        int close_timeout = 3;
        private HotmailPOP confirmator;

        public StdPosterFunctionsNonStatic()
        { 

        }
        // string username, string email, string pwd, string fname
        public bool CreateAccount(Emailaccounts tblaccouts, string proxy, string CreateMacro)
        {
            bool Success = false;
            if (tblaccouts.Email != "")
            {
                iMacros.App m_app = new iMacros.App();
                iMacros.Status s = new iMacros.Status();

                s = m_app.iimOpen("", true, m_timeout);
                
                // Set field values. Some are not existent in some iMacros scripts. 
                // But they are set to cover the full spectrum of iMacros.
                
                s = m_app.iimSet("Username", tblaccouts.Username);
                s = m_app.iimSet("Email", tblaccouts.Email);
                s = m_app.iimSet("Password", tblaccouts.Password);
                s = m_app.iimSet("FirstName", tblaccouts.FirstName);
                s = m_app.iimSet("LastName", tblaccouts.LastName);
                s = m_app.iimSet("BirthdayYYYYMMDD", tblaccouts.Birthday.ToString("yyyy-MM-dd"));
                s = m_app.iimSet("BirthdayDDMMYYYY", tblaccouts.Birthday.ToString("dd/MM/yyyy"));
                s = m_app.iimSet("BirthdayYYYY", tblaccouts.Birthday.ToString("yyyy"));
                
                // FRENCH FORUMS:
                s = m_app.iimSet("StreetFR", tblaccouts.StreetFR);
                s = m_app.iimSet("PostalCodeFR", tblaccouts.PostalCodeFR);
                s = m_app.iimSet("CityFR", tblaccouts.CityFR);

                // AU and DE:
                s = m_app.iimSet("PostalCodeAU", tblaccouts.PostalCodeAU);
                s = m_app.iimSet("PostalCodeUS", tblaccouts.PostalCodeFR);
                s = m_app.iimSet("PostalCodeDE", tblaccouts.PostalCodeDE);

                s = m_app.iimSet("Proxy", proxy);
                s = m_app.iimPlayCode(CreateMacro, m_timeout);
                
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

            }


            // *************************************************
            Success = true; // TEMPORARY VALUE for DEBUGGING 
            // *************************************************

            return Success;
        }
        public string[] CreateAccountWithCaptcha(Emailaccounts tblaccouts, string proxy, string CreateMacro, string topic)
        {
            string[] ErrAndId = new string[2];
        
                iMacros.App m_app = new iMacros.App();
                iMacros.Status s = new iMacros.Status();
                s = m_app.iimOpen("", true, m_timeout);
                /*
                s = m_app.iimSet("Username", username);
                s = m_app.iimSet("Email", email);
                s = m_app.iimSet("Password", pwd);
                s = m_app.iimSet("FirstName", fname);
                s = m_app.iimSet("Proxy", proxy);
                s = m_app.iimSet("Topic", topic);
                */

                s = m_app.iimSet("Topic", topic);

                s = m_app.iimSet("Username", tblaccouts.Username);
                s = m_app.iimSet("Email", tblaccouts.Email);
                s = m_app.iimSet("Password", tblaccouts.Password);
                s = m_app.iimSet("FirstName", tblaccouts.FirstName);
                s = m_app.iimSet("LastName", tblaccouts.LastName);

                s = m_app.iimSet("BirthdayYYYYMMDD", tblaccouts.Birthday.ToString("yyyy-MM-dd"));
                s = m_app.iimSet("BirthdayDDMMYYYY", tblaccouts.Birthday.ToString("dd/MM/yyyy"));
                s = m_app.iimSet("BirthdayYYYY", tblaccouts.Birthday.ToString("yyyy"));

                // FRENCH FORUMS:
                s = m_app.iimSet("StreetFR", tblaccouts.StreetFR);
                s = m_app.iimSet("PostalCodeFR", tblaccouts.PostalCodeFR);
                s = m_app.iimSet("CityFR", tblaccouts.CityFR);

                // AU and US:
                s = m_app.iimSet("PostalCodeAU", tblaccouts.PostalCodeAU);
                s = m_app.iimSet("PostalCodeUS", tblaccouts.PostalCodeFR);
                s = m_app.iimSet("PostalCodeDE", tblaccouts.PostalCodeDE);

                s = m_app.iimSet("Proxy", proxy);
 




                // Execute macro
                s = m_app.iimPlayCode(CreateMacro, m_timeout);
                try
                {
                    ErrAndId[1] = m_app.iimGetExtract(1);
                    ErrAndId[0] = m_app.iimGetExtract(2);
                }
                catch
                {
                }

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
            
            return ErrAndId;
        }
        public void ReportBadCaptcha(string CaptchaId)
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
        public string PostQuestionAskCom(string Email, string Password, string Topic, string Proxy, string Macro)
        {
            // GET iMacros code
            // SET Variables
            // Execute the code
            // Return Success / Failure
            //bool Success = false;

            string QuestionLink = "";

            iMacros.App m_app = new iMacros.App();
            iMacros.Status s = new iMacros.Status();

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
        public bool ConfirmAccount(string email, ref string pwd, string lnkstructure, string sdremail, string proxy, int ShiftLinkBy)
        {
            // GET iMacros code (or use a static code)
            // SET Variables
            // Execute the code
            // Return Success / Failure

            confirmator = new HotmailPOP();

            string LinkExtracted = confirmator.ExtractConfLink(email, pwd, lnkstructure, sdremail, ShiftLinkBy);
            // bool Success = false;

            if (LinkExtracted != null && LinkExtracted != "")
                if (LinkExtracted.Length > 3)
                {
                    if (LinkExtracted.Substring(0, 4) == "http")
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

                        iMacros.App m_app = new iMacros.App();
                        iMacros.Status s = new iMacros.Status();

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

                        return true;
                    }
                    else // if the extracted text is not HTTP, but a password:
                    {
                        pwd = LinkExtracted;
                        return true;
                    }
                }
                else // if the extracted text is not HTTP, but a password:
                {
                    pwd = LinkExtracted;
                    return true;
                }
            else // if linkExtracted = null
            {
                return false;
            }
        }
        private string getCheckifpostedmacro()
        {
            string macrocode = "";
            macrocode = "WAIT SECONDS=1" + Environment.NewLine;
            macrocode += "TAG POS=1 TYPE=A ATTR=TXT:http://tinyurl.com/* EXTRACT=HREF" + Environment.NewLine;
            macrocode += "WAIT SECONDS=1" + Environment.NewLine;
            macrocode += "ADD !EXTRACT {{!URLCURRENT}}" + Environment.NewLine;
            macrocode += "WAIT SECONDS=1" + Environment.NewLine;

            return macrocode;
        }
        public iMacrosPostReturnVars LoginAndPost(string Username, string Email, string Password, string Proxy, string LinkToPost, string GeneratedResponse, string macro)
        {
            string macrocheckifposted = "";
            iMacrosPostReturnVars localposter = new iMacrosPostReturnVars();

            macrocheckifposted = getCheckifpostedmacro();

            iMacros.App m_app = new iMacros.App();
            iMacros.Status s = new iMacros.Status();

            s = m_app.iimOpen("", true, 5);
            s = m_app.iimSet("Username", Username);
            s = m_app.iimSet("Email", Email);
            s = m_app.iimSet("Password", Password);
            s = m_app.iimSet("Proxy", Proxy);
            s = m_app.iimSet("LinkToPost", LinkToPost);
            s = m_app.iimSet("Answer", GeneratedResponse);

            // Execute macro
            s = m_app.iimPlayCode(macro, m_timeout);
            
            s = m_app.iimPlayCode(macrocheckifposted, m_timeout);
            if (m_app.iimGetExtract(1) != "#EANF#") localposter.setSuccess(true);
            else localposter.setSuccess(false);

            if (m_app.iimGetExtract(2) != "NODATA") localposter.setReturnURL(m_app.iimGetExtract(2));
            
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

            return localposter;
        }

        public bool CreateAccountPurseBlog(string username, string email, string pwd, string fname, string proxy, string CreateMacro)
        {

            // STANDARD VARIABLES
            String result = "";
            int errors = 0;

            string ConfirmationTxt = "";
            int inc = 0;
            bool Success = false;
            if (email != "")
            {

                iMacros.App m_app = new iMacros.App();
                iMacros.Status s = new iMacros.Status();

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
        private string SolveCaptchaPurseBlog(string VerificationWord)
        {
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

        public void UpdateProxyStatuses()
        {
            //
            Proxies tblproxies = new Proxies();
            tblproxies.LoadAll();
            int rowcount = tblproxies.RowCount;
            Httpcalls httpcall = new Httpcalls();
            for (int i = 1; i <= rowcount; i++)
            {
                tblproxies.Where.Id.Value = i;
                tblproxies.Where.Id.Operator = WhereParameter.Operand.Equal;
                tblproxies.Query.Load();
                
                if (httpcall.IsAlive(tblproxies.Proxy)) tblproxies.Active = 1;
                else tblproxies.Active = 0;

                tblproxies.Save();
            }
        }
        public bool IsThereMoreThan1WorkingProxy()
        {
            //
            Proxies tblproxies = new Proxies();
            tblproxies.Where.Active.Value = 1;
            tblproxies.Where.Active.Operator = WhereParameter.Operand.Equal;
            tblproxies.Query.Load();
            if (tblproxies.RowCount > 1) return true;
            else return false;
            //
        }
    }

    #region Workresult CLASS
    public class WorkResult
    {
        private int errorNum;
        private String result;
        public WorkResult(String str, int errors)
        {
            errorNum = errors;
            result = str;
        }
        public override String ToString()
        { return result; }
        public int errors()
        { return errorNum; }
    }
    #endregion

    public class iMacrosPostReturnVars
    {
        private bool Success;
        private string ReturnURL;

        public iMacrosPostReturnVars()
        {
            this.Success = false;
            this.ReturnURL = "";
        }
        public bool getSuccess()
        {
            return this.Success;
        }
        public void setSuccess(bool Suc)
        {
            this.Success = Suc;
        }
        public string getReturnURL()
        {
            return this.ReturnURL;
        }
        public void setReturnURL(string url)
        {
            this.ReturnURL = url;
        }
    }


}
