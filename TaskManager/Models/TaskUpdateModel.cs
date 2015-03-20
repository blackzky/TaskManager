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
            : this(BASE_ID++, _taskID, "Update", DateTime.MinValue)
        {
        }

        public TaskUpdateModel(int _id, int _taskID, string _taskUpdate, DateTime _dateUpdated)
        {
            id = _id;
            taskID = _taskID;
            taskUpdate = _taskUpdate;
            dateUpdated = (_dateUpdated == DateTime.MinValue ? DateTime.Now : _dateUpdated);
        }
        public static int GetBaseID()
        {
            return BASE_ID;
        }
        public static void SetBaseID(int base_id)
        {
            BASE_ID = base_id;
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
            if (DateUpdatedDate != null) { dateUpdated = DateUpdatedDate.ToString(DATE_UPDATED_DATE_FORMAT); }
            return dateUpdated;
        }
    }
}
