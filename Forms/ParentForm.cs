using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace iMacrosPostingDashboard
{
    public partial class ParentForm : Form
    {

        BckWorkerTemplate bworker;
        private List<string> cRltBox = new List<string>();
        // private int cPrBar = 0;
        private Dictionary<int, BckWorkerTemplate> workerdict = new Dictionary<int, BckWorkerTemplate>();
        private BindingList<ProjectTableRow> ProjectTable;
        
        private string ProName = "";
        private string TopTable = "";
        private string AnsTemplate = "";
        private int ProId = 0;

        private int pausebfconfirm = 1;
        private int pausebfnextpost = 30;
        // private bool answready = false;
        private bool postQnA = false;

        
        public ParentForm()
        {
            InitializeComponent();
            // InitializeTreeView();
            InitializeDataGrid();
            
            PopulateDataGrid();

            if (this.projectDataGridView1.Rows.Count > 0)
            {
                ProId = (int)this.projectDataGridView1.Rows[0].Cells[0].Value;
                ProName = (string)this.projectDataGridView1.Rows[0].Cells[1].Value;
                TopTable = (string)this.projectDataGridView1.Rows[0].Cells[2].Value;
                AnsTemplate = (string)this.projectDataGridView1.Rows[0].Cells[3].Value;
                pausebfconfirm = (int)this.projectDataGridView1.Rows[0].Cells[8].Value;
                pausebfnextpost = (int)this.projectDataGridView1.Rows[0].Cells[9].Value;
                postQnA = (bool)this.projectDataGridView1.Rows[0].Cells[10].Value;

                this.label2.Text = AnsTemplate;
                // this.projectDataGridView1.Rows[(this.projectDataGridView1.Rows.Count - 1)].Cells[4].Value = "";
            }

        }


        private System.Windows.Forms.DataGridViewTextBoxColumn Idcolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectNamecolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TopicTableNamecolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnswerTemplatecolumn;
        private DataGridViewProgressColumn TopicProgresscolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Statuscolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgressReportcolumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn Languagecolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pausebeforeconfirmcolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pausebeforenextpostcolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostQnAcolumn;

        private void InitializeDataGrid()
        {
        
            this.projectDataGridView1.AutoGenerateColumns = false;

            this.Idcolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectNamecolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopicTableNamecolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnswerTemplatecolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopicProgresscolumn = new iMacrosPostingDashboard.DataGridViewProgressColumn();
            this.Statuscolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressReportcolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.Languagecolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pausebeforeconfirmcolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pausebeforenextpostcolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostQnAcolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.projectDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Idcolumn,
            this.ProjectNamecolumn,
            this.TopicTableNamecolumn,
            this.AnswerTemplatecolumn,
            this.TopicProgresscolumn,
            this.ProgressReportcolumn,
            this.Statuscolumn,
            this.Languagecolumn,
            this.Pausebeforeconfirmcolumn,
            this.Pausebeforenextpostcolumn,
            this.PostQnAcolumn

            });
            
            
            
            
            
            // 
            // Id
            // 
            this.Idcolumn.FillWeight = 19.79696F;
            this.Idcolumn.HeaderText = "Id";
            this.Idcolumn.Name = "Id";
            this.Idcolumn.DataPropertyName = "Id";
            this.Idcolumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ProjectName
            // 
            this.ProjectNamecolumn.FillWeight = 140.1015F;
            this.ProjectNamecolumn.HeaderText = "ProjectName";
            this.ProjectNamecolumn.Name = "ProjectName";
            this.ProjectNamecolumn.DataPropertyName = "ProjectName";
            // 
            // TopicTableName
            // 
            this.TopicTableNamecolumn.HeaderText = "TopicsTable";
            this.TopicTableNamecolumn.Name = "TopicsTable";
            this.TopicTableNamecolumn.DataPropertyName = "TopicsTable";
            this.TopicTableNamecolumn.Visible = false;
            // 
            // AnswerTemplate
            // 
            this.AnswerTemplatecolumn.HeaderText = "AnswerTemplate";
            this.AnswerTemplatecolumn.Name = "AnswerTemplate";
            this.AnswerTemplatecolumn.DataPropertyName = "AnswerTemplate";
            this.AnswerTemplatecolumn.Visible = false;
            // 
            // TopicProgress
            // 
            this.TopicProgresscolumn.FillWeight = 140.1015F;
            this.TopicProgresscolumn.HeaderText = "Topic Progress";
            this.TopicProgresscolumn.Name = "ProgressBar";
            this.TopicProgresscolumn.DataPropertyName = "ProgressBar";
            // 
            // Status
            // 
            this.Statuscolumn.HeaderText = "Proj. Status";
            this.Statuscolumn.Name = "ProjStatus";
            this.Statuscolumn.DataPropertyName = "ProjStatus";
            // 
            // ProgressReport
            // 
            this.ProgressReportcolumn.HeaderText = "ProgressReport";
            this.ProgressReportcolumn.Name = "ProgressReport";
            this.ProgressReportcolumn.DataPropertyName = "ProgressReport";
            this.ProgressReportcolumn.Width = 200;

            this.Languagecolumn.HeaderText = "Language";
            this.Languagecolumn.DataPropertyName = "Language";
            this.Languagecolumn.Visible = true;

            this.Pausebeforeconfirmcolumn.DataPropertyName = "Pausebeforeconfirm";
            this.Pausebeforeconfirmcolumn.Visible = false;

            this.Pausebeforenextpostcolumn.DataPropertyName = "Pausebeforenextpost";
            this.Pausebeforenextpostcolumn.Visible = false;

            this.PostQnAcolumn.DataPropertyName = "PostQnA";
            this.PostQnAcolumn.Visible = false;
        }

        private void InitializeTreeView()
        {
            //
            TreeNode rootNode;
            Projects prjs = new Projects();
            prjs.LoadAll();
            int rownum = prjs.RowCount;
            for (int i = 1; i <= rownum; i++)
            {
                prjs.Filter = Projects.ColumnNames.Id + " = " + i.ToString();
                if (!prjs.EOF)
                {
                    rootNode = new TreeNode(prjs.ProjectName);
                    rootNode.Name = prjs.ProjectName;
                    rootNode.Tag = prjs;
                    treeView1.Nodes.Add(rootNode);                
                }
                prjs.Filter = null;
            }
        }
        private void PopulateDataGrid()
        {
            //DataGridViewProgressColumn column = new DataGridViewProgressColumn();

            this.projectDataGridView1.AutoGenerateColumns = false;

            Projects prjs = new Projects();

            ProjectTable = new BindingList<ProjectTableRow>();

            prjs.Query.AddResultColumn(Projects.ColumnNames.Id);
            prjs.Query.AddResultColumn(Projects.ColumnNames.ProjectName);
            prjs.Query.AddResultColumn(Projects.ColumnNames.TopicsTable);
            prjs.Query.AddResultColumn(Projects.ColumnNames.AnswerTemplate);

            prjs.Query.AddResultColumn(Projects.ColumnNames.Language);
            prjs.Query.AddResultColumn(Projects.ColumnNames.Pausebeforeconfirm);
            prjs.Query.AddResultColumn(Projects.ColumnNames.Pausebeforenextpost);
            prjs.Query.AddResultColumn(Projects.ColumnNames.PostQnA);

            prjs.Query.Load();
            int rownum = prjs.RowCount;
            for (int i = 1; i <= rownum; i++)
            {
                prjs.Filter = Projects.ColumnNames.Id + " = " + i.ToString();

                if (!prjs.EOF)
                {
                    ProjectTableRow row = new ProjectTableRow(prjs.Id, prjs.ProjectName, prjs.TopicsTable, prjs.AnswerTemplate, prjs.Language, prjs.Pausebeforeconfirm, prjs.Pausebeforenextpost, ToBool(prjs.PostQnA));
                    
                    ProjectTable.Add(row);
                }
                prjs.Filter = null;
            }
            this.projectDataGridView1.DataSource = ProjectTable;
        }

        protected void projectDataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int cRowInd = e.RowIndex;
            DataGridViewRow cRow = this.projectDataGridView1.Rows[cRowInd];
            ProId = (int)cRow.Cells[0].Value;
            ProName = (string)cRow.Cells[1].Value;
            TopTable = (string)cRow.Cells[2].Value;
            AnsTemplate = (string)cRow.Cells[3].Value;

            pausebfconfirm = (int)cRow.Cells[8].Value;
            pausebfnextpost = (int)cRow.Cells[9].Value;
            postQnA = (bool)cRow.Cells[10].Value;

            if (workerdict.ContainsKey(ProId)) this.Start1.Enabled = false;
            else this.Start1.Enabled = true;

            this.label2.Text = AnsTemplate;

            // bwork = new BckWorkerTemplate(ProjectName, TopicsTable, AnswerTemplate, ref this.Start1, ref this.ResultBox1, ref this.progressBar1);
            // vMessageBox.Show(ProjectName);
        }

        protected void MDIChildNew_Click(object sender, System.EventArgs e)
        {
            SinglePosterForm newMDIChild = new SinglePosterForm();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }
        protected void MDIChildGenerate_Click(object sender, System.EventArgs e)
        {
            List<SinglePosterForm> ChildFormList = new List<SinglePosterForm>();
            SinglePosterForm newMDIChild;
            Projects prjs = new Projects();
            prjs.LoadAll();
            int rownum = prjs.RowCount;
            int itemindex = 0;
            for (int i = 1; i <= rownum; i++)
            {
                prjs.Filter = Projects.ColumnNames.Id + " = " + i.ToString();
                if (!prjs.EOF)
                {
                    newMDIChild = new SinglePosterForm(prjs.ProjectName, prjs.TopicsTable, prjs.AnswerTemplate);
                    ChildFormList.Add(newMDIChild);
                    itemindex = ChildFormList.Count - 1;
                    // Set the Parent Form of the Child window.
                    ChildFormList[itemindex].MdiParent = this;
                    // Display the new form.
                    ChildFormList[itemindex].Show();
                }
                prjs.Filter = null;
            }

            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }
        protected void CascadeWindows_Click(object sender, System.EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }
        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }
        private void treeView1_NodeSelect(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            this.label2.Text = newSelected.Name;
            // newSelected.Tag
        }
        private void treeview1_Nodekeyboardselect(object sender, TreeViewEventArgs e)
        {
            TreeNode newSelected = this.treeView1.SelectedNode;
            this.label2.Text = newSelected.Name;
            // newSelected.Tag
        }

        private void Start1_Click(object sender, EventArgs e)
        {
            ProjectTableRow CurrentRow = ProjectTable.First(x => x.Id == ProId);
            
            bworker = new BckWorkerTemplate(ProName, TopTable, AnsTemplate, pausebfconfirm, pausebfnextpost, postQnA, ref CurrentRow);
            workerdict.Add(ProId, bworker);
            workerdict[ProId].bwStartDataGrid(sender, e);
            this.Start1.Enabled = false;
            
        }
        private void Stop1_Click(object sender, EventArgs e)
        {
            if (workerdict.ContainsKey(ProId))
            {
                workerdict[ProId].bwStopDataGrid(sender, e);
                workerdict.Remove(ProId);
                this.Start1.Enabled = true;
            }
            else MessageBox.Show("This project is not running. You cannot stop it.");
        }

        private bool ToBool(sbyte x)
        {
            if (x == 0)
                return false;
            else return true;
        }

    }


}
