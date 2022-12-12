using System;
using System.Data;
using System.Threading.Tasks;
using Messenger.Entities;
using Microsoft.Data.SqlClient;

namespace Messenger.Data.Repositories
{
    public class UserFolderRepository
    {
        private readonly SqlConnection _connection;

        public UserFolderRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task Create(UserFolder userFolder)
        {
            using var command = _connection.CreateCommand();

            var folderIdParam = new SqlParameter($"@{nameof(userFolder.FolderId)}", userFolder.FolderId);
            var userIdParam = new SqlParameter($"@{nameof(userFolder.UserId)}", userFolder.UserId);
            command.Parameters.AddRange(new[] { folderIdParam, userIdParam });

            command.CommandText = $"INSERT INTO users_folders (user_id, folder_id) " +
                                  $"VALUES(@{nameof(userFolder.UserId)}, @{nameof(userFolder.FolderId)})";
            await command.ExecuteNonQueryAsync();
        }
    }
}