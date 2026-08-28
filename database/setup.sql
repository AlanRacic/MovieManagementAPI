IF DB_ID(N'MovieManagementDb') IS NULL
BEGIN
    CREATE DATABASE [MovieManagementDb];
END
GO

USE [MovieManagementDb];
GO

IF OBJECT_ID(N'dbo.Movie', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Movie
    (
        ID int IDENTITY(1,1) NOT NULL,
        Title nvarchar(200) NOT NULL,
        Genre nvarchar(50) NULL,
        ReleaseYear int NULL,

        CONSTRAINT PK_Movie
            PRIMARY KEY (ID)
    );
END
GO