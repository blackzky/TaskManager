using CommonUtil;
using System;

namespace TaskManager
{
    public class TaskModel : ObservableObject
    {
        private static int BASE_ID = 0;
        private int id;
        private Priority taskPriority;
        private Status taskStatus;
        private string taskDetail;
        private DateTime deadlineDate;
        private string DEADLINE_DATE_FORMAT = "MM/dd/yy";

        public enum Status : int
        {
            COMPLETED,
            NOT_STARTED,
            WORKING,
            CANCELED
        }
        public enum Priority : int
        {
            HIGH,
            MEDIUM,
            LOW
        }
        
        public TaskModel() 
        {
            id = BASE_ID++;
            taskPriority = Priority.LOW;
            taskStatus = Status.NOT_STARTED;
            taskDetail = "Task";
            deadlineDate = DateTime.Now;
        }
        public int ID 
        {
            get { return id; }
            
        }
        public Priority TaskPriority 
        {
            get { return taskPriority; }
            set
            {
                if (value != taskPriority)
                {
                    taskPriority = value;
                    OnPropertyChanged("TaskPriority");
                }
            }
        }
        public Status TaskStatus 
        {
            get { return taskStatus; }
            set
            {
                if (value != taskStatus)
                {
                    taskStatus = value;
                    OnPropertyChanged("TaskStatus");
                }
            } 
        }
        public string TaskDetail
        {
            get { return taskDetail; }
            set
            {
                if (value != taskDetail)
                {
                    taskDetail = value;
                    OnPropertyChanged("TaskDetail");
                }
            }
        }
        public string Deadline
        {
            get {
                string deadline = "None";
                if (DeadlineDate != null)
                {
                    deadline = DeadlineDate.ToString(DEADLINE_DATE_FORMAT);
                }
                return deadline;
            }
        }
        public DateTime DeadlineDate
        {
            get { 
                return deadlineDate; 
            }
            set
            {
                if (value != deadlineDate)
                {
                    deadlineDate = value;
                    OnPropertyChanged("DeadlineDate");
                }
            }
        }
        public override string ToString()
        {
            return taskDetail;
        }
    }
}
