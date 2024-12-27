
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
CREATE TABLE dbo.Users (
                           Id INT IDENTITY PRIMARY KEY,
                           Username NVARCHAR(100) NOT NULL UNIQUE,
                           PasswordHash NVARCHAR(256) NOT NULL
);
END;


IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'EricAdmin')
BEGIN
INSERT INTO dbo.Users (Username, PasswordHash)
VALUES ('EricAdmin', '$2a$12$eRmYY1UPj0C/5EBjKIFo/u.z2NiT5XIOh2SR4UT/ei4uJ.xLDRaCO'); 
END;


IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'EricUser')
BEGIN
INSERT INTO dbo.Users (Username, PasswordHash)
VALUES ('EricUser', '$2a$12$eRmYY1UPj0C/5EBjKIFo/u.z2NiT5XIOh2SR4UT/ei4uJ.xLDRaCO'); 
END;
