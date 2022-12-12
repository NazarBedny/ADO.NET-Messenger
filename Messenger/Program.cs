using System;
using System.Threading.Tasks;
using Messenger.Data;
using Messenger.Entities;

namespace Messenger
{
    internal class Program
    {
        private static UnitOfWork _unitOfWork;
        private const string ConnectionString = "Server=DESKTOP-RMR3B82\\SQLEXPRESS;Database=messenger;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        static async Task Main(string[] args)
        {
            _unitOfWork = new UnitOfWork(ConnectionString);

            while (true)
            {
                Console.WriteLine("1. Registration\r\n" +
                                  "2. Log-in\r\n");
                int optionNumber;
                while (!int.TryParse(Console.ReadLine(), out optionNumber) ||
                       (optionNumber != 1 && optionNumber != 2))
                {
                    Console.WriteLine("Please make sure that the number is valid\r\n" +
                                      "1. Registration\r\n" +
                                      "2. Log-in\r\n");
                }

                Guid userId = optionNumber switch
                {
                    1 => await Register(),
                    2 => await LogIn()
                };

                var folders = await _unitOfWork.Folders.Get(userId);
                Console.WriteLine("\r\nFolders:");
                foreach (var folder in folders)
                {
                    Console.WriteLine(folder.Name);
                }

                while (true)
                {
                    Console.WriteLine("1. Create new folder\r\n" +
                                      "2. Open a folder\r\n");
                    while (!int.TryParse(Console.ReadLine(), out optionNumber) ||
                           (optionNumber != 1 && optionNumber != 2))
                    {
                        Console.WriteLine("Please make sure that the number is valid\r\n" +
                                          "1. Create new folder\r\n" +
                                          "2. Open a folder\r\n");
                    }

                    switch (optionNumber)
                    {
                        case 1:
                            await CreateFolder(userId); break;
                        case 2: 
                            await OpenFolder(); break;
                    }
                }
            }
        }

        static async Task<Guid> Register()
        {
            Console.WriteLine("Print your name");
            var userName = Console.ReadLine();
            Console.WriteLine("Print your password");
            var password = Console.ReadLine();
            Console.WriteLine("Confirm your password");
            var passwordConfirmation = Console.ReadLine();
            while (passwordConfirmation != password)
            {
                Console.WriteLine("Please make sure that the password match confirm password\r\n" +
                                  "Print your password");
                password = Console.ReadLine();
                Console.WriteLine("Confirm your password");
                passwordConfirmation = Console.ReadLine();
            }
            
            var user = new User
            {
                Username = userName,
                Password = password
            };
            await _unitOfWork.Users.Create(user);

            return user.Id;
        }

        static async Task<Guid> LogIn()
        {
            Console.WriteLine("Print your name");
            var userName = Console.ReadLine();
            Console.WriteLine("Print your password");
            var password = Console.ReadLine();
            var user = await _unitOfWork.Users.Get(userName);

            while (user.Id == Guid.Empty || user.Password != password)
            {
                Console.WriteLine("Please make sure that you entired right credentials\r\n" +
                                  "Print your name");
                userName = Console.ReadLine();
                Console.WriteLine("Print your password");
                password = Console.ReadLine();
                user = await _unitOfWork.Users.Get(userName);
            }

            return user.Id;
        }
        
        static async Task<Guid> CreateFolder(Guid userId)
        {
            Console.WriteLine("Print folder name");
            var name = Console.ReadLine();

            var folder = new Folder
            {
                Name = name
            };
            await _unitOfWork.Folders.Create(folder);
            await _unitOfWork.UsersFolders.Create(new UserFolder { FolderId = folder.Id, UserId = userId });

            var folders = await _unitOfWork.Folders.Get(userId);
            Console.WriteLine("\r\nFolders:");
            foreach (var f in folders)
            {
                Console.WriteLine(f.Name);
            }

            return folder.Id;
        }

        static async Task OpenFolder()
        {
            Console.WriteLine("Print folder name");
            var name = Console.ReadLine();

            var messages = await _unitOfWork.Messages.Get(name);
            
            Console.WriteLine("\r\nMessages:");
            foreach (var message in messages)
            {
                Console.WriteLine(message.Text);
            }
        }

    }
}
