namespace iMacrosPostingDashboard
{
    partial class SinglePosterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ResultBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Stop1 = new System.Windows.Forms.Button();
            this.Start1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ProjectName Poster";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ResultBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.Stop1);
            this.panel1.Controls.Add(this.Start1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(171, 261);
            this.panel1.TabIndex = 14;
            // 
            // ResultBox1
            // 
            this.ResultBox1.BackColor = System.Drawing.Color.Silver;
            this.ResultBox1.Location = new System.Drawing.Point(5, 123);
            this.ResultBox1.Multiline = true;
            this.ResultBox1.Name = "ResultBox1";
            this.ResultBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultBox1.Size = new System.Drawing.Size(159, 82);
            this.ResultBox1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Specific topic progress";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 234);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(159, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // Stop1
            // 
            this.Stop1.Location = new System.Drawing.Point(5, 84);
            this.Stop1.Name = "Stop1";
            this.Stop1.Size = new System.Drawing.Size(159, 23);
            this.Stop1.TabIndex = 5;
            this.Stop1.Text = "Stop";
            this.Stop1.UseVisualStyleBackColor = true;
            this.Stop1.Click += new System.EventHandler(this.Stop1_Click);
            // 
            // Start1
            // 
            this.Start1.Location = new System.Drawing.Point(5, 39);
            this.Start1.Name = "Start1";
            this.Start1.Size = new System.Drawing.Size(159, 23);
            this.Start1.TabIndex = 4;
            this.Start1.Text = "START";
            this.Start1.UseVisualStyleBackColor = true;
            this.Start1.Click += new System.EventHandler(this.Start1_Click);
            // 
            // SinglePosterForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(177, 261);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SinglePosterForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "iMacros - MultiTheadingTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SinglePoster_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox ResultBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Stop1;
        private System.Windows.Forms.Button Start1;
    }
}

