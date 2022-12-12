USE [messenger]
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'users_folders')
BEGIN
    CREATE TABLE [dbo].[users_folders](
        [user_id] UNIQUEIDENTIFIER NOT NULL,
        [folder_id] UNIQUEIDENTIFIER NOT NULL,
     CONSTRAINT [PK_users_folders] PRIMARY KEY CLUSTERED ([user_id], [folder_id]),
     CONSTRAINT [FK_users_folders_folders] FOREIGN KEY ([folder_id])
     REFERENCES [dbo].[folders] ([id])
     ON UPDATE NO ACTION
     ON DELETE NO ACTION, 
     CONSTRAINT [FK_users_folders_users] FOREIGN KEY ([user_id])
     REFERENCES [dbo].[users] ([id])
     ON UPDATE CASCADE
     ON DELETE CASCADE
    )
END