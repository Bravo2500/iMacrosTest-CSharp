using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iMacrosPostingDashboard
{
    public partial class SinglePosterForm : Form
    {

        // BckWorkerTemplate bwork;

        public SinglePosterForm()
        {
            InitializeComponent();
//            bwork = new BckWorkerTemplate("MumsNetUK", "topicsmumsnet", "AmazonUK-one-link", ref this.Start1, ref this.ResultBox1, ref this.progressBar1);

        }
        public SinglePosterForm(string ProjectName, string TopicsTable, string AnswerTemplate)
        {
            // bwork = new BckWorkerTemplate(ProjectName, TopicsTable, AnswerTemplate, ref this.Start1, ref this.ResultBox1, ref this.progressBar1);
        }


//        

        
        private void Start1_Click(object sender, EventArgs e)
        {
            // bwork.bwStart_Click(sender, e);
        }


        private void Stop1_Click(object sender, EventArgs e)
        {
            // bwork.bwStop_Click(sender, e);
        }


        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ************** BEGIN ***************        

        private void SinglePoster_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Start1.Enabled) //  NEED TO MAKE AN ARRAY OUT OF "Start" buttons
            {
                e.Cancel = true;
                MessageBox.Show("Wait until all done.");
            }
        }

        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ********* END ***************        

    }

}
