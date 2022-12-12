using Messenger.Entities;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System;

namespace Messenger.Data.Repositories
{
    public class MessagesRepository
    {
        private readonly SqlConnection _connection;

        public MessagesRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Message>> Get(string folderName)
        {
            var messages = new List<Message>();

            using var command = _connection.CreateCommand();

            var folderNameParam = new SqlParameter($"@{nameof(folderName)}", folderName);
            command.Parameters.Add(folderNameParam);

            command.CommandText = "SELECT * FROM messages " +
                                  "JOIN folders ON folders.id = messages.folder_id " +
                                  $"WHERE folders.name = @{nameof(folderName)}";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new Message
                {
                    Id = reader.GetGuid(0),
                    Text = reader.GetString(1)
                });
            }

            return messages;
        }
    }
}