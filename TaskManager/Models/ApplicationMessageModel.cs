using CommonUtil;
namespace TaskManager.Models
{
    public class ApplicationMessageModel : ObservableObject
    {
        public enum TYPE { INFO, ERROR }
        private enum Color { Skyblue, Red, White }
        private string _message;
        private string _type;

        public ApplicationMessageModel() : this(TYPE.INFO, "") { }
        public ApplicationMessageModel(TYPE type, string msg)
        {
            _message = msg;
            switch (type)
            {
                case TYPE.INFO:
                    _type = Color.Skyblue.ToString();
                    break;
                case TYPE.ERROR:
                    _type = Color.Red.ToString();
                    break;
                default:
                    _type = Color.White.ToString();
                    break;
            }
        }
        public string Message { get { return _message; } }
        public string Type { get { return _type; } } 
    }
}
