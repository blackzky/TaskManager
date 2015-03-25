using System;
namespace TaskManager.Helpers
{
    public class PriorityToImageConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return string.Format("{0}Priority-{1}.png", ResourceManager.IMAGES_BASE_PATH, value);
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
    public class StatusToImageConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return string.Format("{0}Status-{1}.png", ResourceManager.IMAGES_BASE_PATH, value);
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TaskUpdateToTruncatedStringConverter : System.Windows.Data.IValueConverter
    {
        private int TRUNCATE_LENGTH = 50;
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string update = value.ToString();
                string[] subString = update.Split('\r');
                if (subString.Length > 1)
                {
                    update = string.Format("{0} [...]", StringTools.Truncate(subString[0], TRUNCATE_LENGTH));
                }
                else
                {
                    if (update.Length >= TRUNCATE_LENGTH)
                    {
                        update = string.Format("{0} [...]", StringTools.Truncate(update, TRUNCATE_LENGTH));
                    }
                }
                return update;
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
    public class TaskDetailToTruncatedStringConverter : System.Windows.Data.IValueConverter
    {
        private int TRUNCATE_LENGTH = 35;
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string update = value.ToString();
                if (update.Length >= TRUNCATE_LENGTH)
                {
                    update = string.Format("{0} [...]", StringTools.Truncate(update, TRUNCATE_LENGTH));
                }
                return update;
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PriorityToStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                TaskModel.Priority priority = (TaskModel.Priority)Enum.Parse(typeof(TaskModel.Priority), (string)value);
                if (Enum.IsDefined(typeof(TaskModel.Priority), priority) | priority.ToString().Contains(","))
                {
                    return priority;
                }
                else
                {
                    return TaskModel.Priority.Low;
                }
            }
            catch (ArgumentException)
            {
                return TaskModel.Priority.Low;
            }
        }
    }

    public class StatusToStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                TaskModel.Status status = (TaskModel.Status)Enum.Parse(typeof(TaskModel.Status), (string)value);
                if (Enum.IsDefined(typeof(TaskModel.Status), status) | status.ToString().Contains(","))
                {
                    return status;
                }
                else
                {
                    return TaskModel.Status.Unstarted;
                }
            }
            catch (ArgumentException)
            {
                return TaskModel.Status.Unstarted;
            }
        }
    }
}
