using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMacrosPostingDashboard
{
    class TinyURL
    {

        private static int m_timeout = 600;
        private static int close_timeout = 3;
        

        private string GetTinyURLmacro()
        {
            // GETS iMacros Code from DB for execution in the database

            Projects proj = new Projects();
            string code = "";

            if (proj.LoadAll())
            {
                // Iteration walks the DataTable.DefaultView, see the FilterAndSort
                // sample for further clarification.
                proj.Filter = Projects.ColumnNames.ProjectName + " = 'TinyURL'";
                code = proj.ImacrosCodeGeneric;

            }

            return code;
        }

        public string ImacrosTinyurlConvert(string longurl)
        {
            string shorturl = "";

        ShortenAgain:
            iMacros.App m_app = new iMacros.App();
            iMacros.Status s;

            string macro = GetTinyURLmacro();

            s = m_app.iimOpen("", true, 5);
            s = m_app.iimSet("Longurl", longurl);
            // Execute macro
            s = m_app.iimPlayCode(macro, m_timeout);

            try
            {
                shorturl = m_app.iimGetExtract(1);
            }
            catch
            {
                //
                goto ShortenAgain;
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

            // Get variable from macro and return


            return shorturl;
        }

    }

}
