CREATE TABLE [dbo].[UserPasswordHistory](
	[PasswordHistoryId] INT          IDENTITY(1,1) NOT NULL,
	[UserId]            INT          NOT NULL,
	[AuditLogId]        INT          NOT NULL,	
	[Password]          VARCHAR(100) NOT NULL,
	[Creation_Date]     DATETIME     CONSTRAINT [DF_UserPasswordHistory_CreatedDate] DEFAULT (getdate()) NOT NULL,	
	
	CONSTRAINT [PK_UserPasswordHistory] PRIMARY KEY ([PasswordHistoryId] ASC),
	CONSTRAINT [FK_UserPasswordHistory->Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
	CONSTRAINT [FK_UserPasswordHistory->AuditLogs] UNIQUE ([AuditLogId])
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IXQ_UserPasswordHistory_UserId] ON [dbo].[UserPasswordHistory]([UserId] ASC);
GO

