using CommonUtil;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
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
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
            Details = "";
            ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Loading...");

            try
            {
                LoadData();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Failed to load data. " + ResourceManager.PERSISTENT_DATA_PATH_FILENAME + " was not found.");
            }
        }


        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                SaveData();
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

            PersistentData objectToSerialize = new PersistentData();
            objectToSerialize = Serializer.DeSerializeObject(ResourceManager.PERSISTENT_DATA_PATH_FILENAME);
            tasks = objectToSerialize.Tasks;

            foreach (PersistentTaskData task in tasks)
            {
                Task.TaskList.Add(new TaskModel(task.ID, task.TaskPriority, task.TaskStatus, task.TaskDetail, task.DeadlineDate));
            }

            if (Task.TaskList.Count == 0) 
            {
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Task List Empty. Add a Task to start.");
            }
            else
            {
                Task.SelectedTask = Task.TaskList[0];
                ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Application Loaded.");
            }
        }

        private void SaveData() 
        {
            ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "Application closing. Saving data...");

            List<PersistentTaskData> tasks = new List<PersistentTaskData>();

            foreach (TaskModel task in Task.TaskList)
            {
                tasks.Add(new PersistentTaskData(task.ID, task.TaskPriority, task.TaskStatus, task.TaskDetail, task.DeadlineDate));
            }

            PersistentData objectToSerialize = new PersistentData();
            objectToSerialize.Tasks = tasks;

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
