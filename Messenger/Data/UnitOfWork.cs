using Messenger.Data.Repositories;
using Microsoft.Data.SqlClient;

namespace Messenger.Data
{
    public class UnitOfWork
    {
        private readonly UserRepository _userRepository;
        private readonly FolderRepository _folderRepository;
        private readonly UserFolderRepository _usersFoldersRepository;
        private readonly MessagesRepository _messagesRepository;

        public UnitOfWork(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            _userRepository = new UserRepository(connection);
            _folderRepository = new FolderRepository(connection);
            _usersFoldersRepository = new UserFolderRepository(connection);
            _messagesRepository = new MessagesRepository(connection);
        }

        public UserRepository Users => _userRepository;
        public FolderRepository Folders => _folderRepository;
        public UserFolderRepository UsersFolders => _usersFoldersRepository;
        public MessagesRepository Messages => _messagesRepository;
    }
}