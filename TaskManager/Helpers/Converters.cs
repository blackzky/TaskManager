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

    public static class StringTools
    {
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }
}
