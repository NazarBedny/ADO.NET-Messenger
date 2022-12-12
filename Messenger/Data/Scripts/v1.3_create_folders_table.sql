USE [messenger]
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'folders')
BEGIN
	CREATE TABLE [dbo].[folders](
        [id] UNIQUEIDENTIFIER NOT NULL,
        [name] NVARCHAR(max) NOT NULL,
	 CONSTRAINT [PK_folders] PRIMARY KEY CLUSTERED ([id])
	)
END