// Проект использует кастомную админку с GitHub - Core Admin
using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_Core_7_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();// Добавляем сервисы MVC

			// Получаем строку подключения
			string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string user = builder.Configuration["User"];
			string password = builder.Configuration["Password"];
			user = Tools.Decode(new StringBuilder(user));
			password = Tools.Decode(new StringBuilder(password));
			connection = connection.Replace("{0}", user);
			connection = connection.Replace("{1}", password);

            //builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)), ServiceLifetime.Transient, ServiceLifetime.Transient);// Так нужно добавить контекст БД в сервисы чтобы создать ее прямо в этом методе (т. е. еще даже до того, как сервер окончательно запустится и полетят запросы)
            builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
			builder.Services.AddCoreAdmin("Admin");// Подключаем админку. Доступ к ней будет только у ролей "Admin"
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.LoginPath = "/Authentication/Unauthorized";
				options.AccessDeniedPath = "/Authentication/AccessDenied";
			});
			
			var app = builder.Build();
			
			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			
			app.UseRouting();
			
			app.UseAuthentication();
			app.UseAuthorization();
			
			//app.MapControllerRoute(// Как определить маршрут для контроллеров всех областей сразу
			//    name: "Areas",
			//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
			
			//app.MapAreaControllerRoute(// Маршрут для контроллеров конкретной области
			//	name: "AdminArea",
			//	areaName: "Admin",
			//	pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
			//
			//app.MapControllerRoute(// Связываем приходящие от пользователей запросы с контроллерами. Определяем маршрут для контроллеров вне областей
			//	name: "default",// Пока хз
			//	pattern: "{controller=Home}/{action=Post}/{id=0}");// 1-ый параметр шаблона пути определяет контроллер (класс NameController в папке Controllers); 2-ой - открытый метод в этом контроллере (они называются действиями, т. е. методы, способные вызываться клиентом для обработки запроса); 3-ий параметр необязателен и вообще пока хз как его юзать. Т. е. при запросе к корню сформируется запрос "/Home/Index", а APS.NET Core использует для его обработки метод "IActionResult Index()" контроллера "HomeController"

            // В первый раз создаем БД (нужен Transient или Singleton)
            //MyDbContext db = app.Services.GetService<MyDbContext>();
            //db.Database.EnsureCreated();
            //db.Admins.Add(new Admin("MainAdmin", ">ZwTqN314253!") { AdminName = "MainAdmin", AdminAge = 30 });
            //db.SaveChanges();
            //db.Dispose();

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}