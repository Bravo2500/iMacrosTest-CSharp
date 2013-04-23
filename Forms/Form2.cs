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
    public partial class Form2: Form
    {

        System.ComponentModel.BackgroundWorker bw6 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw7 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw8 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw9 = new System.ComponentModel.BackgroundWorker();
        System.ComponentModel.BackgroundWorker bw10 = new System.ComponentModel.BackgroundWorker();

        public Form2()
        {
            InitializeComponent();
            // string macro = Iteration();

            // bw6 Initialization
            bw6.WorkerSupportsCancellation = true;
            bw6.WorkerReportsProgress = true;

            bw6.DoWork += new DoWorkEventHandler(bw6_DoWork);
            bw6.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw6_RunWorkerCompleted);
            bw6.ProgressChanged += new ProgressChangedEventHandler(bw6_ProgressChanged);

            // bw7 Initialization
            bw7.WorkerSupportsCancellation = true;
            bw7.WorkerReportsProgress = true;

            bw7.DoWork += new DoWorkEventHandler(bw7_DoWork);
            bw7.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw7_RunWorkerCompleted);
            bw7.ProgressChanged += new ProgressChangedEventHandler(bw7_ProgressChanged);

            // bw8 Initialization
            bw8.WorkerSupportsCancellation = true;
            bw8.WorkerReportsProgress = true;

            bw8.DoWork += new DoWorkEventHandler(bw8_DoWork);
            bw8.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw8_RunWorkerCompleted);
            bw8.ProgressChanged += new ProgressChangedEventHandler(bw8_ProgressChanged);

            // bw9 Initialization
            bw9.WorkerSupportsCancellation = true;
            bw9.WorkerReportsProgress = true;

            bw9.DoWork += new DoWorkEventHandler(bw9_DoWork);
            bw9.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw9_RunWorkerCompleted);
            bw9.ProgressChanged += new ProgressChangedEventHandler(bw9_ProgressChanged);

            // bw10 Initialization
            bw10.WorkerSupportsCancellation = true;
            bw10.WorkerReportsProgress = true;

            bw10.DoWork += new DoWorkEventHandler(bw10_DoWork);
            bw10.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw10_RunWorkerCompleted);
            bw10.ProgressChanged += new ProgressChangedEventHandler(bw10_ProgressChanged);

        }

        #region bw6
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* bw6 ******* START *************** 

        int totalErrors6 = 0;

        private void Start6_Click(object sender, EventArgs e)
        {
            Start6.Enabled = false;
            totalErrors6 = 0;

            if (bw6.IsBusy != true)
            {
                bw6.RunWorkerAsync();
                ResultBox6.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw6_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            // GenericPoster poster = new GenericPoster(ref sender, ref e, "GuteFrageDE", "Topicsgutefrage");
            // poster.RunSimplePoster();

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw6_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox6.AppendText("Errors occured. Please see log above." + Environment.NewLine);
                totalErrors6++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox6.AppendText("Cancelled." + Environment.NewLine);
                    totalErrors6++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox6.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors6 += result.errors();
                }
            }

            Start6.Enabled = true;
        }

        private void bw6_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar6.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox6.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop6_Click(object sender, EventArgs e)
        {
            if (bw6.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw6.CancelAsync();
                Start6.Enabled = true;
            }
        }

        // ****************** NEW FORM CODE WITH MULTITHREADING ******** bw6 **** END ***************        
        #endregion

        #region bw7
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* bw7 ******* START *************** 

        int totalErrors7 = 0;

        private void Start7_Click(object sender, EventArgs e)
        {
            Start7.Enabled = false;
            totalErrors7 = 0;

            if (bw7.IsBusy != true)
            {
                bw7.RunWorkerAsync();
                ResultBox7.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw7_DoWork(Object sender, DoWorkEventArgs e)
        {

            MumsNetNew.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw7_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox7.AppendText("Errors occured. Please see log above." + Environment.NewLine);
                totalErrors6++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox7.AppendText("Cancelled." + Environment.NewLine);
                    totalErrors6++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox7.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors7 += result.errors();
                }
            }

            Start7.Enabled = true;
        }


        private void bw7_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar7.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox7.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop7_Click(object sender, EventArgs e)
        {
            if (bw7.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw7.CancelAsync();
                Start7.Enabled = true;
            }
        }



        // ****************** NEW FORM CODE WITH MULTITHREADING ******** bw7 **** END ***************        
        #endregion

        #region bw8
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* bw8 ******* START *************** 

        int totalErrors8 = 0;

        private void Start8_Click(object sender, EventArgs e)
        {
            Start8.Enabled = false;
            totalErrors8 = 0;

            if (bw8.IsBusy != true)
            {
                bw8.RunWorkerAsync();
                ResultBox8.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw8_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            FamiljeLivSE.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw8_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox8.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox8.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors8++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox8.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors8++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox8.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors8 += result.errors();
                }
            }

            Start8.Enabled = true;
        }


        private void bw8_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar8.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox8.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop8_Click(object sender, EventArgs e)
        {
            if (bw8.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw8.CancelAsync();
                Start8.Enabled = true;
            }
        }



        // ****************** NEW FORM CODE WITH MULTITHREADING ******** bw8 **** END ***************        

        #endregion

        #region bw9
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* bw9 ******* START *************** 

        int totalErrors9 = 0;

        private void Start9_Click(object sender, EventArgs e)
        {
            Start9.Enabled = false;
            totalErrors9 = 0;

            if (bw9.IsBusy != true)
            {
                bw9.RunWorkerAsync();
                ResultBox9.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw9_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            // AskCom.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw9_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox9.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox9.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors9++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox9.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors9++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox9.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors9 += result.errors();
                }
            }

            Start9.Enabled = true;
        }


        private void bw9_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar9.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox9.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop9_Click(object sender, EventArgs e)
        {
            if (bw9.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw9.CancelAsync();
                Start9.Enabled = true;
            }
        }


        // ****************** NEW FORM CODE WITH MULTITHREADING ******** bw9 **** END ***************        

        #endregion

        #region bw10
        // ****************** NEW FORM CODE WITH MULTITHREADING ********* bw10 ******* START *************** 

        int totalErrors10 = 0;

        private void Start10_Click(object sender, EventArgs e)
        {
            Start10.Enabled = false;
            totalErrors10 = 0;

            if (bw10.IsBusy != true)
            {
                bw10.RunWorkerAsync();
                ResultBox10.AppendText("Running..." + Environment.NewLine);
            }

        }

        private void bw10_DoWork(Object sender, DoWorkEventArgs e)
        {
            //int errors = 0;
            //String result = "";

            PurseBlog.PostNextTopic(sender, e);

            // e.Result = new WorkResult("Done Thread " + e.Argument.ToString() + "; Errors: " +
            // Convert.ToString(errors) + "; Result: " + result, errors);
        }

        private void bw10_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ResultBox10.AppendText("BW stopped. Errors occured." + Environment.NewLine);
                ResultBox10.AppendText(e.Result.ToString() + "" + Environment.NewLine);
                totalErrors10++;
            }
            else
            {
                if (e.Cancelled)
                {
                    ResultBox10.AppendText("Cancelled by user." + Environment.NewLine);
                    totalErrors10++;
                }
                else
                {
                    WorkResult result = e.Result as WorkResult;
                    ResultBox10.AppendText(result.ToString() + Environment.NewLine);
                    totalErrors10 += result.errors();
                }
            }

            Start10.Enabled = true;
        }

        private void bw10_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar10.Value = e.ProgressPercentage;
            string reportmsg = e.UserState as String;
            ResultBox10.AppendText(reportmsg + Environment.NewLine);
        }

        private void Stop10_Click(object sender, EventArgs e)
        {
            if (bw10.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bw10.CancelAsync();
                Start10.Enabled = true;
            }
        }


        // ****************** NEW FORM CODE WITH MULTITHREADING ******** bw10 **** END ***************        

        #endregion

        #region Support Fuctions
        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ************** BEGIN ***************        

        private void Form2FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Start6.Enabled || !Start7.Enabled || !Start8.Enabled || !Start9.Enabled || !Start10.Enabled)
            {
                e.Cancel = true;
                MessageBox.Show("Wait until all done.");
            }
        }

        

        // ****************** SUPPORTING FUNCTIONS FOR MULTITHREADING ********* END ***************        

        #endregion



    }


}
