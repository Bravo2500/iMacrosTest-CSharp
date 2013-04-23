using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;


namespace iMacrosPostingDashboard
{
    public partial class UnitTestingForm : Form
    {
        public UnitTestingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Httpcalls httpcall = new Httpcalls();
            object resp = new Object();
            string[] proxies = textBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string proxy in proxies)
            {
                if (httpcall.IsAlive(proxy)) this.textBox2.Text += proxy + " - ALIVE, " + Environment.NewLine;
                else this.textBox2.Text += proxy + " - DEAD, " + Environment.NewLine;
                this.Refresh();
            }
        }

    }
}
