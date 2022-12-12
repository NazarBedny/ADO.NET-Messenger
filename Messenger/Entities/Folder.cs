using System.Collections.Generic;
using System;

namespace Messenger.Entities
{
    public class Folder
    {
        public Folder()
        {
            UserFolders = new List<UserFolder>();
            Messages = new List<Message>();
            Users = new List<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserFolder> UserFolders { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
}