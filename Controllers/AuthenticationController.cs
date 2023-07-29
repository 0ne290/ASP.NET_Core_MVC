using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP.NET_Core_7_MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Registration([FromServices] MyDbContext db, string? login, string? password, string? name, int age)
        {
            User user = new User() { UserLogin = login, UserPassword = password, UserName = name, UserAge = age };
            db.Users.Add(user);
            db.SaveChanges();
            db.Dispose();
            return Ok(new { Message = "Are you registered" });
        }
        public IActionResult Login([FromServices] MyDbContext db, string? login, string? password)
        {
            User user;
            List<User> users = db.Users.AsNoTracking().ToList();
            db.Dispose();
            user = users.FirstOrDefault(user => Tools.ComputeSha256Hash(login + user.UserGuid + user.UserDate) == user.UserLogin && Tools.ComputeSha256Hash(password + user.UserGuid + user.UserDate) == user.UserPassword);
            if (user == null)
                return BadRequest(new { Message = "Wrong login or password" });
            List<Claim> RfPassportClaims = new List<Claim> { new Claim(ClaimTypes.Name, "RF"), new Claim("IdentityName", "RfPassport"), new Claim("IdentityRole", "User") };
            List<ClaimsIdentity> identities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(RfPassportClaims, "Cookies", "IdentityName", "IdentityRole"),// 3-ий параметр (CookiesName) содержит имя тех утверждений, значения которых будут возвращаться вызовом свойства ClaimsIdentity.Name (в данном случае вызов вернет значение первого найденного утверждения с именем CookiesName), 2-ой - тип аутентификации для удостоверения (в данном случае куки, но это всего лишь строка), 4-ый - роль удостоверения (имеет значение, т. к. может быть использован с атрибутом Authorization)
            };
            ClaimsPrincipal userClient = HttpContext.User;// Добавление к существующим удостоверениям клиента удостоверение пользователя
            userClient.AddIdentities(identities);
            //ClaimsPrincipal userClient = new ClaimsPrincipal(identities);// Сброс всех существующих удостоверений клиента и установка одного удостоверения пользователя
            HttpContext.SignInAsync(userClient);
            return Ok(new { Message = "You are login" });
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "You are logout" });
        }
        [ActionName("Unauthorized")]
        public IActionResult ActionUnauthorized()
        {
            return Unauthorized(new { Message = "You are unauthorized" });
        }
        public IActionResult AccessDenied()
        {
            return Unauthorized(new { Message = "You don't have enough rights" });
        }
    }
}
