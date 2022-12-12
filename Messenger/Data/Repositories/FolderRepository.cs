using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Messenger.Entities;
using Microsoft.Data.SqlClient;

namespace Messenger.Data.Repositories
{
    public class FolderRepository
    {
        private readonly SqlConnection _connection;

        public FolderRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Folder>> Get(Guid userId)
        {
            var folders = new List<Folder>();

            using var command = _connection.CreateCommand();

            var userIdParam = new SqlParameter($"@{nameof(userId)}", userId);
            command.Parameters.Add(userIdParam);

            command.CommandText = "SELECT * FROM folders " +
                                  "JOIN users_folders ON folders.id = users_folders.folder_id " +
                                  $"WHERE users_folders.user_id = @{nameof(userId)}";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                folders.Add(new Folder
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            }

            return folders;
        }

        public async Task Create(Folder folder)
        {
            using var command = _connection.CreateCommand();

            var idParam = new SqlParameter($"@{nameof(folder.Id)}", folder.Id)
                { Direction = ParameterDirection.Output };
            var nameParam = new SqlParameter($"@{nameof(folder.Name)}", folder.Name);
            command.Parameters.AddRange(new[] { idParam, nameParam });

            command.CommandText = $"SET @{nameof(folder.Id)} = NEWID(); " +
                                  $"INSERT INTO folders (id, name) " +
                                  $"VALUES(@{nameof(folder.Id)}, @{nameof(folder.Name)})";
            await command.ExecuteNonQueryAsync();

            folder.Id = (Guid)idParam.Value;
        }
    }
}
