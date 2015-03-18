using CommonUtil;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskManagerViewModel : ObservableObject
    {
        private string _details;
        private ApplicationMessageModel _applicationMessage;

        private TaskViewModel _task;
        private TaskUpdateViewModel _taskUpdate;

        public TaskManagerViewModel()
        {
            Details = "";
            ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Task list is Empty. Add a task to begin.");
        }

        public TaskViewModel Task
        {
            get
            {
                if (_task == null) { _task = new TaskViewModel(this); }
                return _task;
            }
        }
        public TaskUpdateViewModel TaskUpdate
        {
            get
            {
                if (_taskUpdate == null) { _taskUpdate = new TaskUpdateViewModel(this); }
                return _taskUpdate;
            }
        }

        // Helper Methods
        public string Details
        {
            get { return _details; }
            set
            {
                if (value != _details)
                {
                    _details = value;
                    OnPropertyChanged("Details");
                }
            }
        }
        public ApplicationMessageModel ApplicationMessage
        {
            get { return _applicationMessage; }
            set
            {
                if (value != _applicationMessage)
                {
                    _applicationMessage = value;
                    OnPropertyChanged("ApplicationMessage");
                }
            }
        }
        public string Version
        {
            get
            {
                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return string.Format("v{0}", version);
            }
        }
    }

}
