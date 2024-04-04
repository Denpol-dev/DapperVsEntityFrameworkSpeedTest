using Dapper;
using DapperVsEntityFrameworkSpeedTest.Db;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DapperVsEntityFrameworkSpeedTest.Models
{
    public class UserModel
    {
        #region Добавление
        public static async Task<bool> AddUsersEntityFramework()
        {
            using var context = new Context();
            for (int i = 0; i < 1000; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = GetRandomStr(),
                    Password = GetRandomStr(),
                    Email = GetRandomStr(),
                    Sex = GetRandomInt(0, 1),
                    BirthDate = GetRandomDate(),
                };

                var client = new Client
                {
                    ClientId = Guid.NewGuid(),
                    CountSell = GetRandomInt(0, 500),
                    CountBuy = GetRandomInt(0, 500),
                    UserId = user.Id,
                    User = user
                };
                context.Users.Add(user);
                context.Clients.Add(client);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public static async Task<bool> AddUsersDapper(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            var insertUser = "INSERT INTO Users (Id, Name, Password, Email, Sex, BirthDate) VALUES (@Id, @Name, @Password, @Email, @Sex, @BirthDate)";
            var insertClient = "INSERT INTO Clients (ClientId, CountSell, CountBuy, UserId) VALUES (@ClientId, @CountSell, @CountBuy, @UserId)";

            for (int i = 0; i < 1000; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = GetRandomStr(),
                    Password = GetRandomStr(),
                    Email = GetRandomStr(),
                    Sex = GetRandomInt(0, 1),
                    BirthDate = GetRandomDate(),
                };

                var client = new Client
                {
                    ClientId = Guid.NewGuid(),
                    CountSell = GetRandomInt(0, 500),
                    CountBuy = GetRandomInt(0, 500),
                    UserId = user.Id,
                    User = user
                };
                await connection.ExecuteAsync(insertUser, user);
                await connection.ExecuteAsync(insertClient, client);
            }

            return true;
        }

        #endregion

        #region Получение

        public static async Task<(int, int)> GetUsersEntityFramework()
        {
            using var context = new Context();

            var users = await context.Users.Include(x => x.Clients).ToListAsync();

            int usersCount = users.Count;
            int clientCount = 0;
            users.ForEach(u => clientCount += u.Clients.Count);

            return (usersCount, clientCount);
        }

        public static async Task<(int, int)> GetUsersDapper(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            var query = @"SELECT * 
                          FROM Clients c
                          LEFT JOIN Users u on c.UserId = u.Id";

            var users = await connection.QueryAsync<User, Client, User>(query, (user, client) =>
            {
                user.Clients.Add(client);
                return user;
            },
            splitOn: "UserId");

            int usersCount = users.Count();
            int clientCount = 0;

            foreach (var u in users)
            {
                clientCount += u.Clients.Count;
            }

            return (usersCount, clientCount);
        }

        #endregion

        #region Вспомогательные методы

        public static string GetRandomStr()
        {
            int x = GetRandomInt(4, 10);

            string str = "";
            var r = new Random();
            while (str.Length < x)
            {
                char c = (char)r.Next(33, 125);
                if (char.IsLetterOrDigit(c))
                    str += c;
            }
            return str;
        }

        public static int GetRandomInt(int from, int to)
        {
            var dig = new Random();
            int x = dig.Next(from, to);
            return x;
        }

        public static DateTime GetRandomDate()
        {
            var gen = new Random();
            var start = new DateTime(1995, 1, 1);
            int range = (new DateTime(2006, 1, 1) - start).Days;
            return start.AddDays(gen.Next(range));
        }

        #endregion
    }
}
