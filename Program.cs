// ������ ���������� ��������� ������� � GitHub - Core Admin
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
			builder.Services.AddControllersWithViews();// ��������� ������� MVC

			// �������� ������ �����������
			string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string user = builder.Configuration["User"];
			string password = builder.Configuration["Password"];
			user = Tools.Decode(new StringBuilder(user));
			password = Tools.Decode(new StringBuilder(password));
			connection = connection.Replace("{0}", user);
			connection = connection.Replace("{1}", password);

            //builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)), ServiceLifetime.Transient, ServiceLifetime.Transient);// ��� ����� �������� �������� �� � ������� ����� ������� �� ����� � ���� ������ (�. �. ��� ���� �� ����, ��� ������ ������������ ���������� � ������� �������)
            builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
			builder.Services.AddCoreAdmin("Admin");// ���������� �������. ������ � ��� ����� ������ � ����� "Admin"
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
			
			//app.MapControllerRoute(// ��� ���������� ������� ��� ������������ ���� �������� �����
			//    name: "Areas",
			//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
			
			//app.MapAreaControllerRoute(// ������� ��� ������������ ���������� �������
			//	name: "AdminArea",
			//	areaName: "Admin",
			//	pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
			//
			//app.MapControllerRoute(// ��������� ���������� �� ������������� ������� � �������������. ���������� ������� ��� ������������ ��� ��������
			//	name: "default",// ���� ��
			//	pattern: "{controller=Home}/{action=Post}/{id=0}");// 1-�� �������� ������� ���� ���������� ���������� (����� NameController � ����� Controllers); 2-�� - �������� ����� � ���� ����������� (��� ���������� ����������, �. �. ������, ��������� ���������� �������� ��� ��������� �������); 3-�� �������� ������������ � ������ ���� �� ��� ��� �����. �. �. ��� ������� � ����� ������������ ������ "/Home/Index", � APS.NET Core ���������� ��� ��� ��������� ����� "IActionResult Index()" ����������� "HomeController"

            // � ������ ��� ������� �� (����� Transient ��� Singleton)
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