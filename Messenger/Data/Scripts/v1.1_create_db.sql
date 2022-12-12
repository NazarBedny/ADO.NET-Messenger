IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'messenger')
BEGIN
    CREATE DATABASE [messenger]
END