using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_7_MVC.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_7_MVC.Models
{
    public class User
    {
        private string? _userLogin, _userPassword;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]// Т. к. у меня ключ составной, то свойство UserId по умолчанию не будет счётчиком, а этот атрибут явно установит свойству тип "счётчик"
        public int UserId { get; private set; }
        [Key]
        public string? UserGuid { get; private set; }
        public string? UserDate { get; private set; }
        [Required]
        public string? UserLogin
        {
            get
            {
                return _userLogin;
            }
            set
            {
                _userLogin = Tools.ComputeSha256Hash(value + UserGuid + UserDate);
            }
        }
        [Required]
        public string? UserPassword
        {
            get
            {
                return _userPassword;
            }
            set
            {
                _userPassword = Tools.ComputeSha256Hash(value + UserGuid + UserDate);
            }
        }
        public string? UserName { get; set; }
        public int UserAge { get; set; }
        public User(int userId, string? userGuid, string? userDate, string? userLogin, string? userPassword, string? userName, int userAge)
        {
            UserId = userId;
            UserGuid = userGuid;
            UserDate = userDate;
            UserLogin = userLogin;
            UserPassword = userPassword;
            UserName = userName;
            UserAge = userAge;
        }

        public User()
        {
            UserGuid = Guid.NewGuid().ToString();
            UserDate = DateTime.UtcNow.ToString();
            UserName = "StandartUserName";
            UserAge = 0;
        }
    }
    public class Admin
    {
        private string? _adminLogin, _adminPassword;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]// Если бы у меня ключ был составной, то свойство AdminId по умолчанию не было бы счётчиком, а этот атрибут явно установит свойству тип "счётчик"
        public int AdminId { get; private set; }
        [Key]
        public string? AdminGuid { get; private set; }
        public string? AdminDate { get; private set; }
        [Required]
        public string? AdminLogin
        {
            get
            {
                return _adminLogin;
            }
            set
            {
                _adminLogin = Tools.ComputeSha256Hash(value + AdminGuid + AdminDate);
            }
        }
        [Required]
        public string? AdminPassword
        {
            get
            {
                return _adminPassword;
            }
            set
            {
                _adminPassword = Tools.ComputeSha256Hash(value + AdminGuid + AdminDate);
            }
        }
        public string? AdminName { get; set; }
        public int AdminAge { get; set; }
        public Admin(int adminId, string? adminGuid, string? adminDate, string? adminLogin, string? adminPassword, string? adminName, int adminAge)
        {
            AdminId = adminId;
            AdminGuid = adminGuid;
            AdminDate = adminDate;
            AdminLogin = adminLogin;
            AdminPassword = adminPassword;
            AdminName = adminName;
            AdminAge = adminAge;
        }

        public Admin()
        {
            AdminGuid = Guid.NewGuid().ToString();
            AdminDate = DateTime.UtcNow.ToString();
            AdminName = "StandartAdminName";
            AdminAge = 0;
        }// Этот конструктор будет вызываться Entity Framework (к примеру, во время отображения записей таблицы на объекты)
    }
    public class MyDbContext : DbContext// Т. к. классическое использование баз данных в ASP.NET Core налажено через сервисы, то использование конструкторов нежелательно (более того: бессмысленно и вообще вредительство). Почему? А потому что в конструктор ты не передашь никаких параметров, кроме зависимостей (других сервисов). Также можно отказаться и от метода OnConfiguring, т. к. у метода AddDbContext<>(), добавляющего БД в сервисы, есть параметр Action<DbContextOptionsBuilder>, позволяющий манипулировать DbContextOptionsBuilder создаваемого контекста БД также, как и метод OnConfiguring
    {
        public DbSet<DomainLeftPanelListSublistItem> DomainLeftPanelListSublistItem { get; set; } = null!;
        public DbSet<DomainLeftPanelListSublist> DomainLeftPanelListSublists { get; set; } = null!;
        public DbSet<DomainLeftPanelList> LeftPanelLists { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserId);// Для дополнительных удобства и безопасности я сделал в таблице столбец типа "счётчик", но в этой таблице он не является первичным ключом. Это недопустимо для MySQL и EntityFramework, поэтому я сделал этот столбец "альтернативным ключом" (на языке SQL это выльется в команду "CONSTRAINT "AK_Users_UserId" UNIQUE("UserId")")
            modelBuilder.Entity<Admin>().HasAlternateKey(a => a.AdminId);
            //    //modelBuilder.Entity<User>().HasKey(u => new { u.UserId, u.UserGuid, u.UserDate });// Использование составных ключей не позволяет редактировать и удалять записи в CoreAdmin
            //    //modelBuilder.Entity<Admin>().HasKey(a => new { a.AdminId, a.AdminGuid, a.AdminDate });
            //    //modelBuilder.Entity<Admin>().HasData(new Admin("MainAdmin", ">ZwTqN314253!") { AdminId = 1, AdminName = "MainAdmin", AdminAge = 30 });// Это бы прекрасно работало, если бы свойство AdminId не было доступно только для чтения
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)// Эта настройка будет выполняться делегатом в методе AddDbContext() добавления сервиса
        //{
        //    optionsBuilder.UseSqlite("Data Source=helloapp.db");
        //}
    }
}
