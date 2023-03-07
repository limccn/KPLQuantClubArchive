namespace KaiPanLaWeb.Models
{
    public class Message<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T detail { get; set; }
    }
}