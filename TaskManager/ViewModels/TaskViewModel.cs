﻿using CommonUtil;
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
                    _taskManager.TaskUpdate.UpdateTaskUpdatesList();
                    OnPropertyChanged("SelectedTask");
                }
            }
        }
        public ObservableCollection<TaskModel> TaskList
        {
            get 
            {
                if (_taskList == null)
                {
                    _taskList = new ObservableCollection<TaskModel>();
                }
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
            TaskList.Add(newTask);
            _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.INFO, "New task added");
            SelectedTask = newTask;
        }
        /* Incomplete */
        public void RemoveTask()
        {
            if (SelectedTask == null)
            {
                _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "Please select a task to be removed");
            }
            else
            {
                // Show Confirmation Box First
                int taskID = SelectedTask.ID;
                if (TaskList.Remove(SelectedTask))
                {
                    int updatesRemoved = 0;
                    SelectedTask = TaskList.Count == 0 ? null : TaskList[TaskList.Count - 1];
                    updatesRemoved = _taskManager.TaskUpdate.RemoveAllUpdatesOfTask(taskID);

                    _taskManager.ApplicationMessage = new ApplicationMessageModel(
                        ApplicationMessageModel.TYPE.INFO, 
                        string.Format("Task succefully removed. {0} update/s was also removed", updatesRemoved)
                    );
                }
                else
                {
                    _taskManager.ApplicationMessage = new ApplicationMessageModel(ApplicationMessageModel.TYPE.ERROR, "Failed to remove Task.");
                }
            }
        }
    }
}
