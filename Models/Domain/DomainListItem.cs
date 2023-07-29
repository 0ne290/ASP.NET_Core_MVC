namespace ASP.NET_Core_7_MVC.Models.Domain
{
    public class DomainLeftPanelListSublistItem : BodyElement
    {
        private const string _frame = "<Li></Li>";
        public override string Body
        {
            get => _body;
            set
            {
                if (_body != value)// Когда в CoreAdmin редактируется запись, вызывается стандартный конструктор модели с установкой всех ее свойств (измененные свойства получат новые значения, а старые - старые). Таким образом, исходя из логики данного свойства, значение в нём будет дублироваться (с повышением Cursor). Это баг и так не должно быть - поэтому и добавлено условие
                    _body = _frame.Insert(4, value);
            }
        }
    }
}
