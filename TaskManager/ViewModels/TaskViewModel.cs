using CommonUtil;
using System.Collections.ObjectModel;
using System.Diagnostics; /* remove later */
using System.Windows.Input;

namespace TaskManager
{
    public class ApplicationMessage
    {
        public enum TYPE 
        {
            INFO,
            ERROR
        }

        private string _message;
        private string _type;

        public ApplicationMessage()
        {
            _message = "";
            _type = "#FFFFFF";
        }

        public ApplicationMessage(TYPE type, string msg)
        {
            _message = msg;
            switch (type) 
            {
                case TYPE.INFO:
                    _type = "Skyblue";
                    break;
                case TYPE.ERROR:
                    _type = "Red";
                    break;
                default:
                    _type = "White";
                    break;
            }
        }

        public string Message
        {
            get { return _message; }
        }

        public string Type
        {
            get { return _type.ToString(); }
        }
    }


    public class TaskViewModel : ObservableObject
    {
        private TaskModel _selectedTask;

        private ICommand _addTaskCommand;
        private ICommand _removeTaskCommand;

        private ObservableCollection<TaskModel> _taskList;
        private ObservableCollection<TaskUpdateModel> _taskUpdates;

        private string _details;
        private ApplicationMessage _applicationMessage;

        public TaskViewModel ()
        {
            Details = "";
            ApplicationMessage = new ApplicationMessage(ApplicationMessage.TYPE.INFO, "Task list is Empty. Add a task to begin.");

            _taskUpdates = new ObservableCollection<TaskUpdateModel>();

            // 1. Load Task List from DB or FILE
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
        public ApplicationMessage ApplicationMessage
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

        public ObservableCollection<TaskModel> TaskList
        {
            get 
            {
                if (_taskList == null)
                {
                    _taskList = new ObservableCollection<TaskModel>();
                }
                return _taskList;
            }
        }
        public ICommand AddTaskCommand
        {
            get
            {
                if (_addTaskCommand == null)
                {
                    _addTaskCommand = new RelayCommand(
                        param => AddTask()
                    );
                }
                return _addTaskCommand;
            }
        }
        public ICommand RemoveTaskCommand
        {
            get
            {
                if (_removeTaskCommand == null)
                {
                    _removeTaskCommand = new RelayCommand(
                        param => RemoveTask(),
                        param => ((TaskList.Count > 0) && (SelectedTask != null))
                    );
                }
                return _removeTaskCommand;
            }
        }
        public void AddTask ()
        {
            TaskModel newTask = new TaskModel();
            TaskList.Add(newTask);
            ApplicationMessage = new ApplicationMessage(ApplicationMessage.TYPE.INFO, "New task added");
            SelectedTask = newTask;
        }
        public void RemoveTask()
        {
            if (SelectedTask == null)
            {
                ApplicationMessage = new ApplicationMessage(ApplicationMessage.TYPE.ERROR, "Please select a task to be removed");
            }
            else
            {
                // Show Confirmation Box First
                if (TaskList.Remove(SelectedTask))
                {
                    ApplicationMessage = new ApplicationMessage(ApplicationMessage.TYPE.INFO, "Task succefully removed.");
                    SelectedTask = (TaskList.Count > 0) ? TaskList[TaskList.Count - 1] : null;
                }
                else
                {
                    ApplicationMessage = new ApplicationMessage(ApplicationMessage.TYPE.ERROR, "Failed to remove Task.");
                }
            }
        }
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (value != _selectedTask)
                {
                    _selectedTask = value;
                    OnPropertyChanged("SelectedTask");
                }
            }
        }
        public ObservableCollection<TaskUpdateModel> TaskUpdates
        {
            get
            {
                if (_taskUpdates == null)
                {
                    //Temporary -- Seed Data
                    _taskUpdates = new ObservableCollection<TaskUpdateModel>();
                    _taskUpdates.Add(new TaskUpdateModel(0));
                    _taskUpdates.Add(new TaskUpdateModel(1));
                }

                return _taskUpdates;
            }
        }


    }
}
