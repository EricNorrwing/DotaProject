
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
CREATE TABLE Users (
                       Id INT IDENTITY PRIMARY KEY,
                       Username NVARCHAR(100) NOT NULL UNIQUE,
                       Password NVARCHAR(256) NOT NULL
);
END;


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserClaims' AND xtype='U')
BEGIN
CREATE TABLE UserClaims (
                            Id INT IDENTITY PRIMARY KEY,
                            UserId INT NOT NULL,
                            ClaimType NVARCHAR(100) NOT NULL,
                            ClaimValue NVARCHAR(100) NOT NULL,
                            FOREIGN KEY (UserId) REFERENCES Users(Id)
);
END;


IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'EricAdmin')
BEGIN
INSERT INTO Users (Username, Password)
VALUES ('EricAdmin', '$2a$12$eRmYY1UPj0C/5EBjKIFo/u.z2NiT5XIOh2SR4UT/ei4uJ.xLDRaCO'); 

DECLARE @AdminId INT = SCOPE_IDENTITY();

INSERT INTO UserClaims (UserId, ClaimType, ClaimValue)
VALUES (@AdminId, 'role', 'admin');
END;


IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'EricUser')
BEGIN
INSERT INTO Users (Username, Password)
VALUES ('EricUser', '$2a$12$eRmYY1UPj0C/5EBjKIFo/u.z2NiT5XIOh2SR4UT/ei4uJ.xLDRaCO'); 

DECLARE @UserId INT = SCOPE_IDENTITY();

INSERT INTO UserClaims (UserId, ClaimType, ClaimValue)
VALUES (@UserId, 'role', 'verifiedUser');
END;
