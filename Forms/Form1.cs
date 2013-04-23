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
    public partial class Form1 : Form
    {
        System.ComponentModel.BackgroundWorker bw1 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw2 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw3 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw4 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw5 = new System.ComponentModel.BackgroundWorker();
        
        public Form1()
        {
            InitializeComponent();
            // string macro = Iteration();


            // BW1 Initialization
            bw1.WorkerSupportsCancellation = true;
            bw1.WorkerReportsProgress = true;

            bw1.DoWork += new DoWorkEventHandler(bw1_DoWork);
            bw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw1_RunWorkerCompleted);
            bw1.ProgressChanged += new ProgressChangedEventHandler(bw1_ProgressChanged);

            // BW2 Initialization
            bw2.WorkerSupportsCancellation = true;
            bw2.WorkerReportsProgress = true;

            bw2.DoWork += new DoWorkEventHandler(bw2_DoWork);
            bw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw2_RunWorkerCompleted);
            bw2.ProgressChanged += new ProgressChangedEventHandler(bw2_ProgressChanged);

            // BW3 Initialization
            bw3.WorkerSupportsCancellation = true;
            bw3.WorkerReportsProgress = true;

            bw3.DoWork += new DoWorkEventHandler(bw3_DoWork);
            bw3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw3_RunWorkerCompleted);
            bw3.ProgressChanged += new ProgressChangedEventHandler(bw3_ProgressChanged);

            // BW4 Initialization
            bw4.WorkerSupportsCancellation = true;
            bw4.WorkerReportsProgress = true;

            bw4.DoWork += new DoWorkEventHandler(bw4_DoWork);
            bw4.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw4_RunWorkerCompleted);
            bw4.ProgressChanged += new ProgressChangedEventHandler(bw4_ProgressChanged);

            // BW5 Initialization
            bw5.WorkerSupportsCancellation = true;
            bw5.WorkerReportsProgress = true;

            bw5.DoWork += new DoWorkEventHandler(bw5_DoWork);
            bw5.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw5_RunWorkerCompleted);
            bw5.ProgressChanged += new ProgressChangedEventHandler(bw5_ProgressChanged);

        }

        #region BW1
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* BW1 ******* START *************** 

        int totalErrors1 = 0;

        private void Start1_Click(object sender, EventArgs e)
        {
            Start1.Enabled = false;
            totalErrors1 = 0;
            
            if (bw1.IsBusy != true)
            {
                bw1.RunWorkerAsync();
                ResultBox1.AppendText("Running..." + Environment.NewLine);
            }
            
        }

        private void bw1_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            GuteFrageDE.PostNextTopic(sender, e);
            
            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw1_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox1.AppendText("Errors occured. Please see log above." + Environment.NewLine);
                totalErrors1++;
            }
            else 
            {
                if (e.Cancelled)
                {
                    ResultBox1.AppendText("Cancelled." + Environment.NewLine);
                    totalErrors1++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox1.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors1 += result.errors();
                }
            }

            Start1.Enabled = true;   
        }


        private void bw1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox1.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop1_Click(object sender, EventArgs e)
        {
            if (bw1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw1.CancelAsync();
                Start1.Enabled = true;
            }
        }



        // ****************** NEW FORM CODE WITH MULTITHREADING ******** BW1 **** END ***************        
        #endregion

        #region BW2

        // ****************** NEW FORM CODE WITH MULTITHREADING ********* BW2 ******* START *************** 

        int totalErrors2 = 0;

        private void Start2_Click(object sender, EventArgs e)
        {
            Start2.Enabled = false;
            totalErrors2 = 0;

            if (bw2.IsBusy != true)
            {
                bw2.RunWorkerAsync();
                ResultBox2.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw2_DoWork(Object sender, DoWorkEventArgs e)
        {
            GenericPoster poster2 = new GenericPoster(ref sender, ref e, "MumsNetUK", "topicsmumsnet", "AmazonUK-one-link");
            poster2.RunningStatus = true;
            // poster2.RunSimplePoster();

            // MumsNetNew.PostNextTopic(sender, e);
            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw2_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox2.AppendText("Errors occured. Please see log above." + Environment.NewLine);
                totalErrors1++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox2.AppendText("Cancelled." + Environment.NewLine);
                    totalErrors1++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox2.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors2 += result.errors();
                }
            }

            Start2.Enabled = true;
        }

        private void bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar2.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox2.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop2_Click(object sender, EventArgs e)
        {
            if (bw2.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw2.CancelAsync();
                Start2.Enabled = true;
            }
        }



        // ****************** NEW FORM CODE WITH MULTITHREADING ******** BW2 **** END ***************        
        #endregion

        #region BW3
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* BW3 ******* START *************** 

        int totalErrors3 = 0;

        private void Start3_Click(object sender, EventArgs e)
        {
            Start3.Enabled = false;
            totalErrors3 = 0;

            if (bw3.IsBusy != true)
            {
                bw3.RunWorkerAsync();
                ResultBox3.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw3_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            FamiljeLivSE.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw3_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox3.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox3.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors3++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox3.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors3++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox3.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors3 += result.errors();
                }
            }

            Start3.Enabled = true;
        }


        private void bw3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar3.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox3.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop3_Click(object sender, EventArgs e)
        {
            if (bw3.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw3.CancelAsync();
                Start3.Enabled = true;
            }
        }



        // ****************** NEW FORM CODE WITH MULTITHREADING ******** BW3 **** END ***************        

        #endregion

        #region BW4
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* BW4 ******* START *************** 

        int totalErrors4 = 0;

        private void Start4_Click(object sender, EventArgs e)
        {
            Start4.Enabled = false;
            totalErrors4 = 0;

            if (bw4.IsBusy != true)
            {
                bw4.RunWorkerAsync();
                ResultBox4.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw4_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            AskCom.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw4_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox4.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox4.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors4++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox4.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors4++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox4.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors4 += result.errors();
                }
            }

            Start4.Enabled = true;
        }


        private void bw4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar4.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox4.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop4_Click(object sender, EventArgs e)
        {
            if (bw4.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw4.CancelAsync();
                Start4.Enabled = true;
            }
        }


        // ****************** NEW FORM CODE WITH MULTITHREADING ******** BW4 **** END ***************        

        #endregion

        #region BW5
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* BW5 ******* START *************** 

        int totalErrors5 = 0;

        private void Start5_Click(object sender, EventArgs e)
        {
            Start5.Enabled = false;
            totalErrors5 = 0;

            if (bw5.IsBusy != true)
            {
                bw5.RunWorkerAsync();
                ResultBox5.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw5_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            PurseBlog.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw5_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox5.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox5.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors5++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox5.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors5++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox5.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors5 += result.errors();
                }
            }

            Start5.Enabled = true;
        }


        private void bw5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar5.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox5.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop5_Click(object sender, EventArgs e)
        {
            if (bw5.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw5.CancelAsync();
                Start5.Enabled = true;
            }
        }


        // ****************** NEW FORM CODE WITH MULTITHREADING ******** BW5 **** END ***************        

        #endregion

        #region Support Fuctions
        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ************** BEGIN ***************        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Start1.Enabled || !Start2.Enabled || !Start3.Enabled || !Start4.Enabled || !Start5.Enabled)
            {
                e.Cancel = true;
                MessageBox.Show("Wait until all done.");
            }
        }

        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ********* END ***************        

        #endregion


        #region OLD FUCTIONS
        
        private iMacros.App m_app;
        private int m_timeout = 60;

        private void unitTest_Click(object sender, System.EventArgs e)
        {
            // MessageBox.Show(PurseBlog.UnitTest("Visit this verification page. Type out the 1. word without quotation marks:"));

        }

        private void StartStopGuteFragePosting_Click(object sender, System.EventArgs e)
        {

            if (GuteFrageDE.RunningStatus == 0) GuteFrageDE.RunningStatus++;

            if (GuteFrageDE.RunningStatus == 1)
            {
                GuteFrageDE.RunningStatus = GuteFrageDE.RunningStatus - 1;
                // GuteFrageDE.PostNextTopic();
            }
        }


        private void getBtn_Click(object sender, System.EventArgs e)
        {
            Projects proj = new Projects();
            string code = "";
            
            if (proj.LoadAll())
            {
                // Iteration walks the DataTable.DefaultView, see the FilterAndSort
                // sample for further clarification.
                proj.Filter = Projects.ColumnNames.ProjectName + " = 'TinyURL'";
                code = proj.ImacrosCodeGeneric;

            }
            // textBox2.Text = code;
        }

        private void GuteFragePostBtn_Click(object sender, System.EventArgs e)
        {
            // 
            // GuteFrageDE.PostNextTopic();
        }




        private void setBtn_Click(object sender, System.EventArgs e)
        {
            Projects proj = new Projects();
            
            if (proj.LoadAll())
            {
                // Iteration walks the DataTable.DefaultView, see the FilterAndSort
                // sample for further clarification.
                proj.Filter = Projects.ColumnNames.ProjectName + " = 'TinyURL'";
                // proj.ImacrosCodeGeneric = textBox2.Text;
                proj.Save();
            }
        }



        private void tinyURLlaunchBtn_Click(object sender, System.EventArgs e)
        {
            // Get LongURL (by ID)
            string longurl = "";
            Topicsgutefrage tbltopics = new Topicsgutefrage();
            if (tbltopics.LoadAll())
            {
                tbltopics.Filter = Topicsgutefrage.ColumnNames.Id + " = 1";
                longurl = tbltopics.LongURL1;
            }

            // Execute iMacros + LongURL

            // MessageBox.Show(longurl);
            TinyURL TinyClass = new TinyURL();

            string shorturl = TinyClass.ImacrosTinyurlConvert(longurl);

            // MessageBox.Show(shorturl);

            // Save ShortURL (and return Success/Error)
            if (tbltopics.LoadAll())
            {
                tbltopics.Filter = "";
                tbltopics.Filter = Topicsgutefrage.ColumnNames.Id + " = 1";
                tbltopics.ShortURL1 = shorturl;
                tbltopics.Save();
            }
            MessageBox.Show("Done!");

        }



        private void cmdRun_Click(object sender, System.EventArgs e)
        {
            m_app = new iMacros.App();
            iMacros.Status s;
            string macro = Iteration();
            MessageBox.Show(macro);
            // macro = textBox2.Text;
            s = m_app.iimInit("", true, "", "", "", 5);
            s = m_app.iimSet("Username", "Rita");
            s = m_app.iimPlayCode(macro, m_timeout);
            s = m_app.iimExit(m_timeout); 
            /* */
            /// LogStatus(s);
        }

        public string Iteration()
            // EXAMPLE SCRIPT BY  MY-GENERATION SOFTWARE
        {
            Projects emps = new Projects();
            string lastName = "";
            if (emps.LoadAll())
            {
                

                // Iteration walks the DataTable.DefaultView, see the FilterAndSort
                // sample for further clarification.
                do
                    lastName = emps.ProjectName;
                while (emps.MoveNext());

                emps.Rewind();

                do
                    lastName = emps.ProjectName;
                while (emps.MoveNext());
                return lastName;
            }
            return lastName;

            //-----------------------------------------------------------
            // Moral: 
            //-----------------------------------------------------------
            // Iteration is simple, you can rewind and restart at any time
            //-----------------------------------------------------------
        }

#endregion OLD FUCTIONS

    }

}
