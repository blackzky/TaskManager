using CommonUtil;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager
{
    public class TaskViewModel : ObservableObject
    {
        private TaskManagerViewModel _taskManager;

        private TaskModel _selectedTask;
        private ObservableCollection<TaskModel> _taskList;
        private List<TaskModel> _allTasks;

        private ICommand _addTaskCommand;
        private ICommand _removeTaskCommand;

        private bool _hasTaskSelected;
        private bool _taskListUpdated = false;

        private int _filter;
        private Dictionary<string, int> _filterList;

        public TaskViewModel(TaskManagerViewModel taskManager)
        {
            _taskManager = taskManager;

            _filterList = new Dictionary<string, int>();
            _filterList.Add("Low", 64);         //1000000
            _filterList.Add("Medium", 32);      //0100000
            _filterList.Add("High", 16);        //0010000
            _filterList.Add("Unstarted", 8);    //0001000
            _filterList.Add("Working", 4);      //0000100
            _filterList.Add("Completed", 2);    //0000010
            _filterList.Add("Cancelled", 1);    //0000001
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
                    HasTaskSelected = (_selectedTask != null);
                    _taskManager.TaskUpdate.UpdateTaskUpdatesList();
                }
            }
        }
        public ObservableCollection<TaskModel> TaskList
        {
            get 
            {
                if (_taskList == null) _taskList = new ObservableCollection<TaskModel>();
                return _taskList;
            }
        }
        public List<TaskModel> AllTaskLists
        {
            get
            {
                if (_allTasks == null) _allTasks = new List<TaskModel>();
                return _allTasks;
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
            newTask.PropertyChanged += TaskModel_PropertyChanged;
            AllTaskLists.Add(newTask);
            TaskListUpdated = true;
            if (IsFiltered(newTask))
            {
                _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "New task added.");
                SelectedTask = newTask;
            }
            else
            {
                _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "New task added but is not visible because of filter");
                SelectedTask = TaskList.Count == 0 ? null : TaskList[TaskList.Count - 1];
            }
        }
        public void RemoveTask()
        {
            if (SelectedTask == null)
            {
                _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "Please select a task to be removed");
            }
            else
            {
                System.Windows.MessageBoxResult removeChoice = System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "Are you sure you want to remove task?", "Remove Task", System.Windows.MessageBoxButton.YesNoCancel);
                if (removeChoice != System.Windows.MessageBoxResult.Yes) return;

                int taskID = SelectedTask.ID;
                SelectedTask.PropertyChanged -= TaskModel_PropertyChanged;
                if (AllTaskLists.Remove(SelectedTask))
                {
                    int updatesRemoved = 0;
                    TaskListUpdated = true;
                    SelectedTask = TaskList.Count == 0 ? null : TaskList[TaskList.Count - 1];
                    updatesRemoved = _taskManager.TaskUpdate.RemoveAllUpdatesOfTask(taskID);

                    string message = "Task succefully removed. ";
                    if (updatesRemoved > 0)
                    {
                        message = string.Format("{0} {1} Task Update{2} was also removed.", message, updatesRemoved, (updatesRemoved > 1 ? "s" : ""));
                    }
                    if (AllTaskLists.Count == 0)
                    {
                        message = string.Format("{0} {1} Task Update{2} was also removed. Task list is now empty.", message, updatesRemoved, (updatesRemoved > 1 ? "s" : ""));
                    }

                    _taskManager.ApplicationMessage = new ApplicationMessageModel(
                        ApplicationMessageModel.TYPE.INFO,
                        message
                    );
                }
                else
                {
                    _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "Failed to remove Task.");
                }
            }
        }
        public bool TaskListUpdated
        {
            get { return _taskListUpdated; }
            set
            {
                if (value == true)
                {
                    SortList();
                    _taskListUpdated = false;
                    OnPropertyChanged("TaskListUpdated");
                }
            }
        }
        public bool HasTaskSelected {
            get { return _hasTaskSelected; }
            set
            {
                if (value != _hasTaskSelected)
                {
                    _hasTaskSelected = value;
                    OnPropertyChanged("HasTaskSelected");
                }
            }
        }
        //<summary>
        // This function will compare Task by Priority
        // Returns: int
        //      -1 = X < Y
        //       0 = X == Y
        //       1 = X > Y
        //</summary>
        public int CompareByPriority (TaskModel x, TaskModel y)
        {
            if (x == null || y == null) return 0;

            TaskModel.Priority xp = x.TaskPriority;
            TaskModel.Priority yp = y.TaskPriority;
            if (xp == yp)
            {
                return 0;
            }
            else
            {
                if (xp == TaskModel.Priority.Low) return -1;
                else if (xp == TaskModel.Priority.Medium && yp == TaskModel.Priority.Low) return 1;
                else if (xp == TaskModel.Priority.Medium && yp == TaskModel.Priority.High) return -1;
                else if (xp == TaskModel.Priority.High) return 1;
                else return 0;
            }
        }
        private void SortList()
        {
            System.Collections.Generic.List<TaskModel> myList = new System.Collections.Generic.List<TaskModel>(AllTaskLists);
            myList.Sort(CompareByPriority);
            myList.Reverse();

            TaskList.Clear();
            bool _filtered = false;
            foreach (TaskModel task in myList) 
            {
                _filtered = IsFiltered(task);
                if (_filtered)
                {
                    task.PropertyChanged += TaskModel_PropertyChanged;
                    TaskList.Add(task);
                }
            }
        }
        // Returns true if task is filtered (task will be shown)
        private bool IsFiltered(TaskModel task) 
        {
            try
            {
                int priorityFilter = _filterList[task.TaskPriority.ToString()];
                int statusFilter = _filterList[task.TaskStatus.ToString()];

                if ((statusFilter & Filter) > 0) return true;
                if ((priorityFilter & Filter) > 0) return true;
            }
            catch (System.Exception)
            {
                return false;
            }

            return false;
        }
        public int Filter
        {
            get { return _filter; }
            set 
            {
                _filter = _filter ^ value; /* [^ - XOR] - This will toggle the certain bit of Filter */
                OnPropertyChanged("Filter");
                TaskListUpdated = true;
                SelectedTask = TaskList.Count == 0 ? null : TaskList[TaskList.Count - 1];
            }
        }
        private string GetIntBinaryString(int n)
        {
            char[] b = new char[7];
            int pos = 6;
            int i = 0;

            while (i < 7)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        private void TaskModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TaskPriority") TaskListUpdated = true;
            if (e.PropertyName == "TaskStatus") TaskListUpdated = true;
            if (e.PropertyName == "TaskDetail")
            {
                if (SelectedTask != null && SelectedTask.TaskDetail == "")
                {
                    _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "Task Detail should not be empty.");
                }
                else
                {
                    if (_taskManager.ApplicationMessage.Message != "") _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "");
                }
            }
        }
    }
}
