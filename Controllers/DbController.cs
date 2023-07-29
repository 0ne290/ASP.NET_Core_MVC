using ASP.NET_Core_7_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ASP.NET_Core_7_MVC.Controllers
{
    public class DbController : Controller
    {
        private MyDbContext db;
        public DbController(MyDbContext pDb)
        {
            db = pDb;
        }
        public IActionResult Recreate()
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Admins.Add(new Admin() { AdminLogin = "MainAdmin", AdminPassword = ">ZwTqN314253!", AdminName = "MainAdmin", AdminAge = 30 });
            db.SaveChanges();
            db.Dispose();
            return Ok(new { Message = "Recreate database is complete" });
        }
        [HttpGet, ActionName("User")]
        public IActionResult UserGet(int id = 0)
        {
            List<User> res = db.Users.AsNoTracking().ToList();
            db.Dispose();
            if (id > 0)
                return Json(res.FirstOrDefault(x => x.UserId == id));
            return Json(res, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Admin(int id = 0)
        {
            List<Admin> res = db.Admins.AsNoTracking().ToList();
            db.Dispose();
            if (id > 0)
                return Json(res.FirstOrDefault(x => x.AdminId == id));
            return Json(res, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
        [HttpPost, ActionName("User")]
        public IActionResult UserPost(int length = 1)
        {
            length = length < 1 ? 1 : length;
            db.Users.AddRange(Tools.UserRandom(length));
            db.SaveChanges();
            db.Dispose();
            return Ok(new { Message = $"Adding {length} random records to the \"Users\" table is complete" });
        }
    }
}
