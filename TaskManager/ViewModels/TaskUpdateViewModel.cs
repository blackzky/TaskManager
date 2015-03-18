using CommonUtil;
using System.Collections.ObjectModel;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskUpdateViewModel : ObservableObject
    {
        private TaskManagerViewModel _taskManager;

        private TaskUpdateModel _selectedTaskUpdate;
        private ObservableCollection<TaskUpdateModel> _taskUpdates;

        public TaskUpdateViewModel(TaskManagerViewModel taskManager)
        {
            _taskManager = taskManager;
            _taskUpdates = new ObservableCollection<TaskUpdateModel>();
        }

        public TaskUpdateModel SelectedTaskUpdate
        {
            get { return _selectedTaskUpdate; }
            set
            {
                if (value != _selectedTaskUpdate)
                {
                    _selectedTaskUpdate = value;
                    OnPropertyChanged("SelectedTaskUpdate");
                }
            }
        }
        public ObservableCollection<TaskUpdateModel> TaskUpdates
        {
            get
            {
                if (_taskUpdates == null)
                {
                    //Temporary -- Seed Data
                    _taskUpdates = new ObservableCollection<TaskUpdateModel>();
                    _taskUpdates.Add(new TaskUpdateModel(0));
                    _taskUpdates.Add(new TaskUpdateModel(1));
                }

                return _taskUpdates;
            }
        }
    }
}
