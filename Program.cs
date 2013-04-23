using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iMacrosPostingDashboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParentForm());
            // Application.Run(new UnitTestingForm());
            
        }
    }
}
