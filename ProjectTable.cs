using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace iMacrosPostingDashboard
{

    public class ProjectTableRow : INotifyPropertyChanged
    {
        private int _id;
        private string _projectName;
        private string _topicsTable;
        private string _answerTemplate;
        private int _progressbar;
        private string _projStatus;
        private string _progressReport;
        private string _language;

        private int _pausebeforeconfirm; 
        private int _pausebeforenextpost;
        private bool _postQnA;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectTableRow(int id, string projectname, string topicstable, string answertemplate, string language, int pausebeforeconfirm, int pausebeforenextpost, bool postQnA)
        {
            _id = id;
            _projectName = projectname;
            _topicsTable = topicstable;
            _answerTemplate = answertemplate;
            _progressbar = 0;
            _projStatus = "Not running"; // + Environment.NewLine;
            _progressReport = "None";
            _language = language;
            _pausebeforeconfirm = pausebeforeconfirm; 
            _pausebeforenextpost = pausebeforenextpost;
            _postQnA = postQnA;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        public string TopicsTable
        {
            get { return _topicsTable; }
            set { _topicsTable = value; }
        }
        public string AnswerTemplate
        {
            get { return _answerTemplate; }
            set { _answerTemplate = value; }
        }
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        public int Pausebeforeconfirm
        {
            get { return _pausebeforeconfirm; }
            set { _pausebeforeconfirm = value; }
        }
        public int Pausebeforenextpost
        {
            get { return _pausebeforenextpost; }
            set { _pausebeforenextpost = value; }
        }
        public bool PostQnA
        {
            get { return _postQnA; }
            set { _postQnA = value; }
        }

        // Notify when the following properties change
        public int ProgressBar
        {
            get { return _progressbar; }
            set
            {
              _progressbar = value;
              this.NotifyPropertyChanged("ProgressBar");
            }
        }
        public string ProjStatus
        {
            get { return _projStatus; }
            set
            {
                _projStatus = value;
                this.NotifyPropertyChanged("ProjStatus");
            }
        }
        public string ProgressReport
        {
            get { return _progressReport; }
            set
            {
                _progressReport = value;
                this.NotifyPropertyChanged("ProgressReport");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

}

