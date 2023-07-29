using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace ASP.NET_Core_7_MVC.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Registration([FromServices] MyDbContext db, string? login, string? password, string? name, int age)
        {
            Admin admin = new Admin() { AdminLogin = login, AdminPassword = password, AdminName = name, AdminAge = age };
            db.Admins.Add(admin);
            db.SaveChanges();
            db.Dispose();
            return Ok(new { Message = "Administrator registered" });
        }
        public IActionResult Login([FromServices] MyDbContext db, string? login, string? password)
        {
            Admin admin;
            List<Admin> admins = db.Admins.AsNoTracking().ToList();
            db.Dispose();
            admin = admins.FirstOrDefault(admin => Tools.ComputeSha256Hash(login + admin.AdminGuid + admin.AdminDate) == admin.AdminLogin && Tools.ComputeSha256Hash(password + admin.AdminGuid + admin.AdminDate) == admin.AdminPassword);
            if (admin == null)
                return BadRequest(new { Message = "Wrong login or password" });
            List<Claim> UkPassportClaims = new List<Claim> { new Claim(ClaimTypes.Name, "UK"), new Claim("IdentityName", "UkPassport"), new Claim("IdentityRole", "Admin") };
            List<ClaimsIdentity> identities = new List<ClaimsIdentity>
            {
			    new ClaimsIdentity(UkPassportClaims, "Cookies", "IdentityName", "IdentityRole")
            };
            ClaimsPrincipal userClient = HttpContext.User;// Добавление к существующим удостоверениям клиента удостоверение админа
            userClient.AddIdentities(identities);
            //ClaimsPrincipal userClient = new ClaimsPrincipal(identities);// Сброс всех существующих удостоверений клиента и установка одного удостоверения админа
            HttpContext.SignInAsync(userClient);
            return Ok(new { Message = "You are login" });
        }
    }
}
