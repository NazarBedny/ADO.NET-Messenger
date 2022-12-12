USE [messenger]
GO 
    
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'users')
BEGIN
    CREATE TABLE [dbo].[users](
        [id] UNIQUEIDENTIFIER NOT NULL DEFAULT (NEWID()),
        [username] NVARCHAR(50) NOT NULL,
        [password] NVARCHAR(MAX) NOT NULL
     CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([id]),
     CONSTRAINT [UQ_users] UNIQUE NONCLUSTERED ([username])
    )
END