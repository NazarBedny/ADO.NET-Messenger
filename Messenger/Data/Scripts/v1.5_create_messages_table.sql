USE [messenger]
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'messages')
BEGIN
    CREATE TABLE [dbo].[messages](
        [id] UNIQUEIDENTIFIER NOT NULL,
        [text] NVARCHAR(max) NOT NULL,
        [folder_id] UNIQUEIDENTIFIER NOT NULL,
     CONSTRAINT [PK_messages] PRIMARY KEY CLUSTERED ([id]),
     CONSTRAINT [FK_messages_folders] FOREIGN KEY ([folder_id])
     REFERENCES [dbo].[folders] ([id])
     ON UPDATE NO ACTION
     ON DELETE NO ACTION
    )
END