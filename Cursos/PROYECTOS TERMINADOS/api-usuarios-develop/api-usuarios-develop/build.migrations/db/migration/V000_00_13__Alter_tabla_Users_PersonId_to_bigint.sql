BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT [FK_Users->UserStatuses]
GO
ALTER TABLE dbo.UserStatuses SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_Users_UserStatus
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_Users_LoginAttempts
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_Users_CreatedDate
GO
CREATE TABLE dbo.Tmp_Users
	(
	UserId int NOT NULL IDENTITY (1, 1),
	PersonId bigint NOT NULL,
	DocumentCountryId int NULL,
	DocumentTypeId int NOT NULL,
	DocumentNumber varchar(20) NOT NULL,
	Username varchar(100) NOT NULL,
	Password varchar(100) NOT NULL,
	UserStatusId tinyint NOT NULL,
	LoginAttempts int NULL,
	LastPasswordChange datetime NULL,
	LastLogon datetime NULL,
	CreatedDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Users SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Users ADD CONSTRAINT
	DF_Users_UserStatus DEFAULT ((1)) FOR UserStatusId
GO
ALTER TABLE dbo.Tmp_Users ADD CONSTRAINT
	DF_Users_LoginAttempts DEFAULT ((0)) FOR LoginAttempts
GO
ALTER TABLE dbo.Tmp_Users ADD CONSTRAINT
	DF_Users_CreatedDate DEFAULT (getdate()) FOR CreatedDate
GO
SET IDENTITY_INSERT dbo.Tmp_Users ON
GO
IF EXISTS(SELECT * FROM dbo.Users)
	 EXEC('INSERT INTO dbo.Tmp_Users (UserId, PersonId, DocumentCountryId, DocumentTypeId, DocumentNumber, Username, Password, UserStatusId, LoginAttempts, LastPasswordChange, LastLogon, CreatedDate)
		SELECT UserId, CONVERT(bigint, PersonId), DocumentCountryId, DocumentTypeId, DocumentNumber, Username, Password, UserStatusId, LoginAttempts, LastPasswordChange, LastLogon, CreatedDate FROM dbo.Users WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Users OFF
GO
ALTER TABLE dbo.UserPasswordHistory
	DROP CONSTRAINT [FK_UserPasswordHistory->Users]
GO
ALTER TABLE dbo.UserUsernameHistory
	DROP CONSTRAINT [FK_UserUsernameHistory->Users]
GO
DROP TABLE dbo.Users
GO
EXECUTE sp_rename N'dbo.Tmp_Users', N'Users', 'OBJECT' 
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	PK_Users PRIMARY KEY CLUSTERED 
	(
	UserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	UC_Users_DocumentFields UNIQUE NONCLUSTERED 
	(
	DocumentCountryId,
	DocumentTypeId,
	DocumentNumber
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	UC_Users_UsernameWithDocNumber UNIQUE NONCLUSTERED 
	(
	DocumentNumber,
	Username
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	[FK_Users->UserStatuses] FOREIGN KEY
	(
	UserStatusId
	) REFERENCES dbo.UserStatuses
	(
	UserStatusId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserUsernameHistory ADD CONSTRAINT
	[FK_UserUsernameHistory->Users] FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserUsernameHistory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserPasswordHistory ADD CONSTRAINT
	[FK_UserPasswordHistory->Users] FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserPasswordHistory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
