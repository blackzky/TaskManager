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
}
