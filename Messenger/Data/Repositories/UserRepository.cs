using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using Messenger.Entities;

namespace Messenger.Data.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> Get(string username)
        {
            var user = new User();

            using var command = _connection.CreateCommand();

            var idParam = new SqlParameter($"@{nameof(username)}", username);
            command.Parameters.Add(idParam);

            command.CommandText = $"SELECT * FROM users " +
                    $"WHERE username = @{nameof(username)}";

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                user.Id = reader.GetGuid(0);
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
            }

            return user;
        }

        public async Task Create(User user)
        {
            using var command = _connection.CreateCommand();

            var idParam = new SqlParameter($"@{nameof(user.Id)}", user.Id) { Direction = ParameterDirection.Output };
            var usernameParam = new SqlParameter($"@{nameof(user.Username)}", user.Username);
            var passwordParam = new SqlParameter($"@{nameof(user.Password)}", user.Password);
            command.Parameters.AddRange(new[] { idParam, usernameParam, passwordParam });

            command.CommandText = $"SET @{nameof(user.Id)} = NEWID(); " +
                    $"INSERT INTO users (id, username, password) " +
                    $"VALUES(@{nameof(user.Id)}, @{nameof(user.Username)}, @{nameof(user.Password)})";

            await command.ExecuteNonQueryAsync();

            user.Id = (Guid)idParam.Value;
        }
    }
}