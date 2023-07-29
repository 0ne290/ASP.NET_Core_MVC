namespace ASP.NET_Core_7_MVC.Models.Domain
{
    abstract public class BodyElement : BaseElement
    {
        protected string _body;
        public abstract string Body { get; set; }
    }
}
