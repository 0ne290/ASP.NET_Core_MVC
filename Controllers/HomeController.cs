using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ASP.NET_Core_7_MVC.Models.Domain;
using System.Text;

namespace ASP.NET_Core_7_MVC.Controllers
{
    // [NonController] - этот атрибут убирает класс из списка контроллеров; [Controller] - этот атрибут добавляет класс в список контроллеров; [Route("CustomHome")] - для всех действий этого контроллера "корневым" адресом (префиксом в URL) стал бы "CustomHome" вместо стандартного "Home"
    public class HomeController : Controller// Класс становится контроллером при соблюдении одного из трех следующих условий: в имени класса есть суффикс "Controller"; класс унаследован от класса, который имеет суффикс "Controller"; к классу применяется атрибут [Controller]
    {
        //private readonly ILogger<HomeController> _logger;
        //
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        // [NonAction] - этот атрибут убирает метод из списка действий; [ActionName("Welcome")] - этот атрибут изменяет имя действия (т. е. к методу "HomeController.Index()" можно будет обратиться по адресу "/Home/Welcome"); [HttpPost] - этот атрибут делает метод обработчиком только POST-запросов 
		// [Route("CustomAbout")] - если к контроллеру явно применён атрибут "Route", задающий корневой каталог, то действие будет доступно по адресу "RootCatalog/CustomAbout" в противном случае "/CustomAbout"
        public IActionResult About()// Классы, реализующие интерфейс IActionResult, специальным образом формируют ответ (к примеру, возвращают отформатированный JSON, обычный текст, устанавливают коды ответов или отправляют файл). Также можно сделать свою реализацию интерфейса IActionResult (к примеру, для формирования HTML-документа - вызываешь метод от строкового аргумента и эта строка помещается в тег body заранее определенного документа)
        {
            ViewBag.Title = "Amogus";// "Приоритет" (на самом деле просто порядок переопределений) параметров представлений: представление, мастер-страница, действие
            return View();// Метод, унаследованный от класса Controller. Этот метод отправит в качестве ответа соответствующее представление (APS.NET Core связывает IActionResult-методы контроллеров с представлениями по следующему шаблону: Controllers/NameController.Method() --> Views/Name/Method.cshtml). Т. е. в данном случае получим представление "Views/Home/Index.cshtml". UPD: это метод расширения; раскрывается он в "new ViewResult()" (как раз объект типа IActionResult)
        }
        public IActionResult Post([FromServices] MyDbContext db, int id)// Классы, реализующие интерфейс IActionResult, специальным образом формируют ответ (к примеру, возвращают отформатированный JSON, обычный текст, устанавливают коды ответов или отправляют файл). Также можно сделать свою реализацию интерфейса IActionResult (к примеру, для формирования HTML-документа - вызываешь метод от строкового аргумента и эта строка помещается в тег body заранее определенного документа)
        {
            int[] threads = new int[1] { 3 };
            System.Collections.ArrayList shell1 = new System.Collections.ArrayList { HttpContext, 0, threads }, shell2 = new System.Collections.ArrayList { HttpContext, 0, threads }, shell3 = new System.Collections.ArrayList { HttpContext, 0, threads };
            ThreadPool.QueueUserWorkItem(new WaitCallback(Tools.Read<DomainLeftPanelList>), shell1);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Tools.Read<DomainLeftPanelListSublist>), shell2);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Tools.Read<DomainLeftPanelListSublistItem>), shell3);
            while (threads[0] != 0)
                Thread.Sleep(0);
            List<DomainLeftPanelList> dlpls = (List<DomainLeftPanelList>)shell1[1];
            List<DomainLeftPanelListSublist> dlplss = (List<DomainLeftPanelListSublist>)shell2[1];
            List<DomainLeftPanelListSublistItem> dlplsis = (List<DomainLeftPanelListSublistItem>)shell3[1];
            ViewBag.Nav = new StringBuilder("", 1024);
            foreach (DomainLeftPanelListSublist dlpl1 in dlplss)
            {
                foreach (DomainLeftPanelListSublistItem dlplsi in dlplsis)
                {
                    if (dlplsi.Link.Contains(dlpl1.Codeword + ";"))
                        dlpl1.Add(dlplsi.Body);
                }
            }
            foreach (DomainLeftPanelList dlpl in dlpls)
            {
                foreach (DomainLeftPanelListSublist dlpl1 in dlplss)
                {
                    if (dlpl1.Link.Contains(dlpl.Codeword + ";"))
                        dlpl.Add(dlpl1.Body);
                }
                ViewBag.Nav.Append(dlpl.Body);
            }
            ViewBag.Nav = ViewBag.Nav.ToString();
            db.Dispose();
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