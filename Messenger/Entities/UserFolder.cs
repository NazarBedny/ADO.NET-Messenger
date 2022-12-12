using System;

namespace Messenger.Entities
{
    public class UserFolder
    {
        public Guid UserId { get; set; }
        public Guid FolderId { get; set; }

        public User User { get; set; }
        public Folder Folder { get; set; }
    }
}