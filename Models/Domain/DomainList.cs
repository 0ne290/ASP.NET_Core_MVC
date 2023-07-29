namespace ASP.NET_Core_7_MVC.Models.Domain
{
    public class DomainLeftPanelList : BodyElement
    {
        private const string _frame = "<Div class=\"Text Text_Title-Navigation\"></Div><Ul class=\"Text Navigation\" id=\"Main-Navigation\"></Ul>";
        public int Cursor { get; private set; }
        public override string Body
        {
            get => _body;
            set
            {
                if (_body != value)// Когда в CoreAdmin редактируется запись, вызывается стандартный конструктор модели с установкой всех ее свойств (измененные свойства получат новые значения, а старые - старые). Таким образом, исходя из логики данного свойства, значение в нём будет дублироваться (с повышением Cursor). Это баг и так не должно быть - поэтому и добавлено условие
                {
                    _body = _frame.Insert(40, value);
                    Cursor += value.Length;
                }
            }
        }
        public DomainLeftPanelList(string body, int cursor)
        {
            Cursor = cursor;
            Body = body;
        }
        public DomainLeftPanelList()
        {
            Cursor = 95;
        }

        public void Add(string term)
        {
            _body = Body.Insert(Cursor, term);
            Cursor += term.Length;
        }
    }
}
