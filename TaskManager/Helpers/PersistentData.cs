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
        //private List<PersistentTaskUpdateData> tasksUpdates;

        public List<PersistentTaskData> Tasks
        {
            get { return this.tasks; }
            set { this.tasks = value; }
        }

        /*
        public List<PersistentTaskUpdateData> TasksUpdates
        {
            get { return this.tasksUpdates; }
            set { this.tasksUpdates = value; }
        }
         */

        public PersistentData()
        { 
        }

        public PersistentData(SerializationInfo info, StreamingContext ctxt)
        {
            System.Diagnostics.Debug.WriteLine("--PersistentData--");
            tasks = (List<PersistentTaskData>)info.GetValue("Tasks", typeof(List<PersistentTaskData>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            System.Diagnostics.Debug.WriteLine("--PersistentData.GetObjectData--");
            info.AddValue("Tasks", tasks);
        }
    }

    [Serializable()]
    public class PersistentTaskUpdateData
    {

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
