namespace ASP.NET_Core_7_MVC.Models.Domain
{
    abstract public class BaseElement
    {
        public int Id { get; private set; }
        public string Codeword { get; set; }
        public string Link { get; set; }
    }
}
