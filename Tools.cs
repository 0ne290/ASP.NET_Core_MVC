using ASP.NET_Core_7_MVC.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_7_MVC
{
    public class Tools
    {
        public static void Read<Table>(object shell) where Table : class
        {
            System.Collections.ArrayList args = (System.Collections.ArrayList)shell;
            HttpContext context = (HttpContext)args[0];
            IServiceScope scp = context.RequestServices.CreateScope();
            MyDbContext db = scp.ServiceProvider.GetService<MyDbContext>();
            args[1] = db.Set<Table>().AsNoTracking().ToList();// Если бы тут не было AsNoTracking(), то я бы получил отслеживаемые объекты. Во-первых, это гораздо дольше. А во-вторых, изменение таких получаемых отслеживаемых объектов отразилось бы и на соответствующих записях в БД (после SaveChanges(), разумеется)
            db.Dispose();
            scp.Dispose();
            int[] threads = (int[])args[2];
            Interlocked.Decrement(ref threads[0]);
        }
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static string Encode(StringBuilder text)// Этот метод кодирования использует тот же алгоритм, что и метод декодирования логина-пароля из конфига в начале файла Program.cs. Сам проект этот метод кодирования не использует - он просто представлен тут для описания того, как же можно получить корректно закодированные данные для строки подключения
        {
            int code;
            for (int i = 0; i < text.Length; i++)
            {
                code = (int)text[i];
                if (code % 2 == 1)
                    code += 50;
                else
                    code *= 4;
                text[i] = (char)code;
            }
            return text.ToString();
        }
        public static string Decode(StringBuilder text)
		{
			int code;
			for (int i = 0; i < text.Length; i++)
			{
				code = (int)text[i];
				if (code % 2 == 1)
					code -= 50;
				else
					code /= 4;
				text[i] = (char)code;
			}
			return text.ToString();
		}
        public static string StringRandom(int length)
        {
            Random randomizer = new Random();
            char[] letters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz".ToCharArray();
            char[] res = new char[length];
            for (int i = 0; i < length; i++)
                res[i] = letters[randomizer.Next(52)];
            return new string(res);
        }
        public static User[] UserRandom(int length)// Метод формирования массива случайных записей
        {
            Random randomizer = new Random();
            User[] res = new User[length];
            for (int i = 0; i < length; i++)
                res[i] = new User() { UserLogin = StringRandom(10), UserPassword = StringRandom(10), UserAge = randomizer.Next(100), UserName = StringRandom(10) };
            return res;
        }
    }
}
