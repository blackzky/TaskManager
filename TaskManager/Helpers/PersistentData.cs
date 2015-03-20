using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//http://tech.pro/tutorial/618/csharp-tutorial-serialize-objects-to-a-file
namespace TaskManager.Helpers
{
    [Serializable()]
    public class PersistentData : ISerializable
    {
        private List<PersistentTaskData> tasks;
        private List<PersistentTaskUpdateData> taskUpdates;

        private int lastTaskID;
        private int lastTaskUpdateID;

        public int LastTaskID
        {
            get { return lastTaskID; }
            set { lastTaskID = value; }
        }
        public int LastTaskUpdateID
        {
            get { return lastTaskUpdateID; }
            set { lastTaskUpdateID = value; }
        }

        public List<PersistentTaskData> Tasks
        {
            get { return this.tasks; }
            set { this.tasks = value; }
        }
        public List<PersistentTaskUpdateData> TaskUpdates
        {
            get { return this.taskUpdates; }
            set { this.taskUpdates = value; }
        }

        public PersistentData()
        { 
        }

        public PersistentData(SerializationInfo info, StreamingContext ctxt)
        {
            lastTaskID = (int)info.GetValue("LastTaskID", typeof(int));
            lastTaskUpdateID = (int)info.GetValue("LastTaskUpdateID", typeof(int));

            tasks = (List<PersistentTaskData>)info.GetValue("Tasks", typeof(List<PersistentTaskData>));
            taskUpdates = (List<PersistentTaskUpdateData>)info.GetValue("TaskUpdates", typeof(List<PersistentTaskUpdateData>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("LastTaskID", lastTaskID);
            info.AddValue("LastTaskUpdateID", lastTaskUpdateID);

            info.AddValue("Tasks", tasks);
            info.AddValue("TaskUpdates", taskUpdates);
        }
    }

    [Serializable()]
    public class PersistentTaskUpdateData : ISerializable
    {
        private int id;
        private int taskId;
        private string taskUpdateDetail;
        private DateTime dateUpdated;

        public int ID { get { return id; } }
        public int TaskID { get { return taskId; } }
        public string TaskUpdateDetail { get { return taskUpdateDetail; } }
        public DateTime DateUpdated { get { return dateUpdated; } }

        public PersistentTaskUpdateData() 
        {

        }

        public PersistentTaskUpdateData(SerializationInfo info, StreamingContext ctxt)
        {
            id = (int)info.GetValue("ID", typeof(int));
            taskId = (int)info.GetValue("TaskID", typeof(int));
            taskUpdateDetail = (string)info.GetValue("TaskUpdateDetail", typeof(string));
            dateUpdated = (DateTime)info.GetValue("DateUpdated", typeof(DateTime));
        }

        public PersistentTaskUpdateData(int _id, int _taskID, string _taskUpdateDetail, DateTime _dateUpdated) 
        {
            id = _id;
            taskId = _taskID;
            taskUpdateDetail = _taskUpdateDetail;
            dateUpdated = _dateUpdated;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", id);
            info.AddValue("TaskID", taskId);
            info.AddValue("TaskUpdateDetail", taskUpdateDetail);
            info.AddValue("DateUpdated", dateUpdated);
        }
    }

    [Serializable()]
    public class PersistentTaskData: ISerializable
    {
        private int id;
        private TaskManager.TaskModel.Priority taskPriority;
        private TaskManager.TaskModel.Status taskStatus;
        private string taskDetail;
        private DateTime deadlineDate;

        public int ID { get { return id; } }
        public TaskManager.TaskModel.Priority TaskPriority { get { return taskPriority; } }
        public TaskManager.TaskModel.Status TaskStatus { get { return taskStatus; } }
        public string TaskDetail { get { return taskDetail; } }
        public DateTime DeadlineDate { get { return deadlineDate; } }

        public PersistentTaskData() 
        {

        }

        public PersistentTaskData(SerializationInfo info, StreamingContext ctxt)
        {
            id = (int)info.GetValue("ID", typeof(int));
            taskPriority = (TaskManager.TaskModel.Priority)info.GetValue("TaskPriority", typeof(TaskManager.TaskModel.Priority));
            taskStatus = (TaskManager.TaskModel.Status)info.GetValue("TaskStatus", typeof(TaskManager.TaskModel.Status));
            taskDetail = (string)info.GetValue("TaskDetail", typeof(string));
            deadlineDate = (DateTime)info.GetValue("DeadlineDate", typeof(DateTime));
        }

        public PersistentTaskData(int _id, TaskManager.TaskModel.Priority _taskPriority, TaskManager.TaskModel.Status _taskStatus, string _taskDetail, DateTime _deadlineDate) 
        {
            id = _id;
            taskPriority = _taskPriority;
            taskStatus = _taskStatus;
            taskDetail = _taskDetail;
            deadlineDate = _deadlineDate;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", id);
            info.AddValue("TaskPriority", taskPriority);
            info.AddValue("TaskStatus", taskStatus);
            info.AddValue("TaskDetail", taskDetail);
            info.AddValue("DeadlineDate", deadlineDate);
        }

    }
}
