using CommonUtil;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskManagerViewModel : ObservableObject
    {
        private ApplicationMessageModel _applicationMessage;

        private TaskViewModel _task;
        private TaskUpdateViewModel _taskUpdate;

        private ICommand _filterTaskCommand;

        public TaskManagerViewModel()
        {
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
            ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Loading...");

            try
            {
                LoadData();
            }
            catch (System.Exception)
            {
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Failed to load data.");
            }
        }

        public ICommand FilterTaskCommand
        {
            get
            {
                if (_filterTaskCommand == null)
                {
                    _filterTaskCommand = new RelayCommand(
                        param => FilterTask(param)
                    );
                }
                return _filterTaskCommand;
            }
        }
        private void FilterTask(object o) 
        {
            Task.Filter = System.Convert.ToInt32(o.ToString(), 2);
            if (Task.Filter == 0 && Task.AllTaskLists.Count > 0)
            {
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, string.Format("Task list is not empty. Please select a filter to show them.", Task.AllTaskLists.Count));
            } 
            else
            {
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, string.Format("{0} Tasks filtered out of {1}", Task.TaskList.Count, Task.AllTaskLists.Count));
            }
        }
        

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (Task.AllTaskLists.Count == 0)
                {
                    MessageBoxResult exitChoice = MessageBox.Show(Application.Current.MainWindow, "Task list is empty, do you want to save empty list?", "", MessageBoxButton.YesNoCancel);

                    switch (exitChoice)
                    {
                        case MessageBoxResult.Yes: SaveData(); break;
                        case MessageBoxResult.Cancel: e.Cancel = true; break;
                        case MessageBoxResult.No: default: break;
                    }
                }
                else {
                    SaveData(); 
                }
            }
            catch(System.Exception ex)
            {
                e.Cancel = true;
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Failed to save data. " + StringTools.Truncate(ex.Message, 50));
            }
        }

        private void LoadData()
        {
            List<PersistentTaskData> tasks = new List<PersistentTaskData>();
            List<PersistentTaskUpdateData> taskUpdates = new List<PersistentTaskUpdateData>();

            PersistentData objectToSerialize = new PersistentData();
            objectToSerialize = Serializer.DeSerializeObject(ResourceManager.PERSISTENT_DATA_PATH_FILENAME);
            tasks = objectToSerialize.Tasks;
            taskUpdates = objectToSerialize.TaskUpdates;

            TaskModel.SetBaseID(objectToSerialize.LastTaskID);
            TaskUpdateModel.SetBaseID(objectToSerialize.LastTaskUpdateID);

            if (tasks != null && tasks.Count > 0)
            {
                foreach (PersistentTaskData task in tasks)
                {
                    Task.AllTaskLists.Add(new TaskModel(task.ID, task.TaskPriority, task.TaskStatus, task.TaskDetail, task.DeadlineDate));
                }
            }

            if (taskUpdates != null && taskUpdates.Count > 0)
            {
                foreach (PersistentTaskUpdateData taskUpdate in taskUpdates)
                {
                    TaskUpdate.AllTaskUpdates.Add(new TaskUpdateModel(taskUpdate.ID, taskUpdate.TaskID, taskUpdate.TaskUpdateDetail, taskUpdate.DateUpdated));
                }

            }
            Task.Filter = 127;
            if (Task.AllTaskLists.Count == 0) 
            {
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Task List Empty. Add a Task to start.");
            }
            else
            {
                Task.SelectedTask = Task.AllTaskLists[0];
                ApplicationMessage = new ApplicationMessageModel(
                    ApplicationMessageModel.TYPE.INFO, 
                    string.Format(
                        "{0} Tasks was loaded. {1}", 
                        Task.AllTaskLists.Count, 
                        (Task.TaskList.Count == Task.AllTaskLists.Count ? "" : string.Format("Showing {0} filtered tasks.", Task.TaskList.Count))
                    )
                );
            }
        }

        private void SaveData() 
        {
            ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Application closing. Saving data...");

            List<PersistentTaskData> tasks = new List<PersistentTaskData>();
            List<PersistentTaskUpdateData> taskUpdates = new List<PersistentTaskUpdateData>();

            foreach (TaskModel task in Task.AllTaskLists)
            {
                tasks.Add(new PersistentTaskData(task.ID, task.TaskPriority, task.TaskStatus, task.TaskDetail, task.DeadlineDate));
            }

            foreach (TaskUpdateModel taskUpdated in TaskUpdate.AllTaskUpdates)
            {
                taskUpdates.Add(new PersistentTaskUpdateData(taskUpdated.ID, taskUpdated.TaskID, taskUpdated.TaskUpdate, taskUpdated.DateUpdatedDate));
            }

            PersistentData objectToSerialize = new PersistentData();
            objectToSerialize.Tasks = tasks;
            objectToSerialize.TaskUpdates = taskUpdates;

            objectToSerialize.LastTaskID = TaskModel.GetBaseID();
            objectToSerialize.LastTaskUpdateID = TaskUpdateModel.GetBaseID();

            Serializer.SerializeObject(ResourceManager.PERSISTENT_DATA_PATH_FILENAME, objectToSerialize);
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
