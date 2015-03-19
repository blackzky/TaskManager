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
        private string DATE_UPDATED_DATE_FORMAT = "MM/dd/yy";
        private string deadline;

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
                    DateUpdatedDate = DateTime.Now;
                    OnPropertyChanged("TaskUpdate");
                }
            }
        }
        public DateTime DateUpdatedDate
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
        public string DateUpdated
        {
            get
            {
                deadline = GetDateUpdated();
                return deadline;
            }
            set
            {
                if (value != deadline)
                {
                    deadline = value;
                    OnPropertyChanged("Deadline");
                }
            }
        }
        
        private string GetDateUpdated()
        {
            string dateUpdated = "None";
            System.Diagnostics.Debug.WriteLine("date udpated...");
            if (DateUpdatedDate != null) { dateUpdated = DateUpdatedDate.ToString(DATE_UPDATED_DATE_FORMAT); }
            return dateUpdated;
        }
    }
}
