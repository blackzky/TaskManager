using CommonUtil;
using System;

namespace TaskManager
{
    public class TaskUpdateModel : ObservableObject
    {
        private static int BASE_ID = 0;
        private int id;
        private int taskID;
        private string taskUpdate;
        private DateTime dateUpdated;

        public TaskUpdateModel(int _taskID)
        {
            id = BASE_ID++;
            taskID = _taskID;
            taskUpdate = "Update";
            dateUpdated = DateTime.Now;
        }

        public int ID
        {
            get { return id; }
        }

        public int TaskID
        {
            get { return taskID; }
        }

        public string TaskUpdate
        {
            get { return taskUpdate; }
            set
            {
                if (value != taskUpdate)
                {
                    taskUpdate = value;
                    OnPropertyChanged("TaskUpdate");
                }
            }
        }

        public DateTime DateUpdated
        {
            get { return dateUpdated; }
            set
            {
                if (value != dateUpdated)
                {
                    dateUpdated = value;
                    OnPropertyChanged("DateUpdated");
                }
            }
        }
    }
}
