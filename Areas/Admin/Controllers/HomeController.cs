using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NET_Core_7_MVC.Areas.Admin.Controllers
{
    // [NonController] - этот атрибут убирает класс из списка контроллеров; [Controller] - этот атрибут добавляет класс в список контроллеров; [Route("CustomHome")] - для всех действий этого контроллера "корневым" адресом (префиксом в URL) стал бы "CustomHome" вместо стандартного "Home"
    [Area("Admin")]
    public class HomeController : Controller// Класс становится контроллером при соблюдении одного из трех следующих условий: в имени класса есть суффикс "Controller"; класс унаследован от класса, который имеет суффикс "Controller"; к классу применяется атрибут [Controller]
    {
        //private readonly ILogger<HomeController> _logger;
        //
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        // [NonAction] - этот атрибут убирает метод из списка действий; [ActionName("Welcome")] - этот атрибут изменяет имя действия (т. е. к методу "HomeController.Index()" можно будет обратиться по адресу "/Home/Welcome"); [HttpPost] - этот атрибут делает метод обработчиком только POST-запросов 
        public IActionResult Index()// Классы, реализующие интерфейс IActionResult, специальным образом формируют ответ (к примеру, возвращают отформатированный JSON, обычный текст, устанавливают коды ответов или отправляют файл). Также можно сделать свою реализацию интерфейса IActionResult (к примеру, для формирования HTML-документа - вызываешь метод от строкового аргумента и эта строка помещается в тег body заранее определенного документа)
        {
            ViewBag.Title = "Amogus";// "Приоритет" (на самом деле просто порядок переопределений) параметров представлений: представление, мастер-страница, действие
            return View();// Метод, унаследованный от класса Controller. Этот метод отправит в качестве ответа соответствующее представление (APS.NET Core связывает IActionResult-методы контроллеров с представлениями по следующему шаблону: Controllers/NameController.Method() --> Views/Name/Method.cshtml). Т. е. в данном случае получим представление "Views/Home/Index.cshtml". UPD: это метод расширения; раскрывается он в "new ViewResult()" (как раз объект типа IActionResult)
        }
		// [Route("CustomAbout")] - если к контроллеру явно применён атрибут "Route", задающий корневой каталог, то действие будет доступно по адресу "RootCatalog/CustomAbout" в противном случае "/CustomAbout"
        public IActionResult About()// Классы, реализующие интерфейс IActionResult, специальным образом формируют ответ (к примеру, возвращают отформатированный JSON, обычный текст, устанавливают коды ответов или отправляют файл). Также можно сделать свою реализацию интерфейса IActionResult (к примеру, для формирования HTML-документа - вызываешь метод от строкового аргумента и эта строка помещается в тег body заранее определенного документа)
        {
            ViewBag.Title = "Amogus";// "Приоритет" (на самом деле просто порядок переопределений) параметров представлений: представление, мастер-страница, действие
            return View();// Метод, унаследованный от класса Controller. Этот метод отправит в качестве ответа соответствующее представление (APS.NET Core связывает IActionResult-методы контроллеров с представлениями по следующему шаблону: Controllers/NameController.Method() --> Views/Name/Method.cshtml). Т. е. в данном случае получим представление "Views/Home/Index.cshtml". UPD: это метод расширения; раскрывается он в "new ViewResult()" (как раз объект типа IActionResult)
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}
        //
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}