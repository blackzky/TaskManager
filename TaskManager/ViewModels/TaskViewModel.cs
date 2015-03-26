using CommonUtil;
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

        private ICommand _addTaskCommand;
        private ICommand _removeTaskCommand;

        private bool _hasTaskSelected;

        private bool _taskListUpdated = false;

        public TaskViewModel(TaskManagerViewModel taskManager)
        {
            _taskManager = taskManager;
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
            TaskList.Add(newTask);
            _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "New task added");
            TaskListUpdated = true;
            SelectedTask = newTask;
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
                if (TaskList.Remove(SelectedTask))
                {
                    int updatesRemoved = 0;
                    TaskListUpdated = true;
                    SelectedTask = TaskList.Count == 0 ? null : TaskList[TaskList.Count - 1];
                    updatesRemoved = _taskManager.TaskUpdate.RemoveAllUpdatesOfTask(taskID);

                    string message = "Task succefully removed. ";
                    if (updatesRemoved > 0)
                    {
                        message = string.Format("{0} {1} Task Update{2} was also removed", message, updatesRemoved, (updatesRemoved > 1 ? "s" : ""));
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
                if (value != _taskListUpdated)
                {
                    _taskListUpdated = value;
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
            System.Collections.Generic.List<TaskModel> myList = new System.Collections.Generic.List<TaskModel>(_taskList);
            myList.Sort(CompareByPriority);
            myList.Reverse();

            TaskList.Clear();
            foreach (TaskModel task in myList) 
            {
                TaskList.Add(task);
            }
        }

        private void TaskModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TaskPriority") TaskListUpdated = true;
        }
    }
}
