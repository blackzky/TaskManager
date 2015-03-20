using CommonUtil;
using System;
using System.Collections.Generic;

namespace TaskManager
{
    public class TaskModel : ObservableObject
    {
        private static int BASE_ID = 0;
        private int id;
        private Priority taskPriority;
        private Status taskStatus;
        private string taskDetail;
        private string deadline;
        private DateTime deadlineDate;
        private string DEADLINE_DATE_FORMAT = "MM/dd/yy";

        public enum Status : int
        {
            NOT_STARTED = 0,
            WORKING = 1,
            COMPLETED = 2,
            CANCELLED = 3
        }
        public enum Priority : int
        {
            LOW = 0,
            MEDIUM = 1,
            HIGH = 2
        }
        
        public IEnumerable<string> PriorityEnum { get; set; }
        public IEnumerable<string> StatusEnum { get; set; }

        public TaskModel()
            : this(BASE_ID++, Priority.LOW, Status.NOT_STARTED, "Task", DateTime.MinValue) { }

        //<summary>
        // For Persistent Data.
        //</summary>
        public TaskModel(int _id, Priority _taskPriority, Status _taskStatus, string _taskDetail, DateTime _deadlineDate)
        {
            id = _id;
            taskPriority = _taskPriority;
            taskStatus = _taskStatus;
            taskDetail = _taskDetail;

            DeadlineDate = (_deadlineDate == DateTime.MinValue ? DateTime.Now.AddDays(-1) : _deadlineDate);

            var priorityEnum = Enum.GetNames(typeof(Priority));
            PriorityEnum = priorityEnum;

            var statusEnum = Enum.GetNames(typeof(Status));
            StatusEnum = statusEnum;
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
            get { return deadline; }
            set 
            {
                if (value != deadline)
                {
                    deadline = value;
                    OnPropertyChanged("Deadline");
                }
            }
        }
        public DateTime DeadlineDate
        {
            get { return deadlineDate; }
            set
            {
                if (value != deadlineDate)
                {
                    deadlineDate = value;
                    Deadline = GetDeadline();
                    OnPropertyChanged("DeadlineDate");
                }
            }
        }
        private string GetDeadline() 
        {
            string _deadline = "None";
            if (DeadlineDate != null)
            {
                int diff = DateTime.Compare(DateTime.Now.Date, DeadlineDate.Date);
                if (diff <= 0) 
                {
                    _deadline = DeadlineDate.ToString(DEADLINE_DATE_FORMAT);
                }
            }
            return _deadline;
        }

        public override string ToString()
        {
            return taskDetail;
        }
    }
}
