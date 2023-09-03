CREATE TABLE [dbo].[UserUsernameHistory](
	[UsernameHistoryId] INT          IDENTITY(1,1) NOT NULL,
	[UserId]            INT          NOT NULL,
	[AuditLogId]        INT          NOT NULL,	
	[Username]          VARCHAR(100) NOT NULL,
	[CreationDate]		DATETIME     CONSTRAINT [DF_UserUsernameHistory_CreatedDate] DEFAULT (getdate()) NOT NULL,	
	
	CONSTRAINT [PK_UserUsernameHistory] PRIMARY KEY ([UsernameHistoryId] ASC),
	CONSTRAINT [FK_UserUsernameHistory->Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
	CONSTRAINT [FK_UserUsernameHistory->AuditLogs] FOREIGN KEY ([AuditLogId]) REFERENCES [dbo].[AuditLogs] ([AuditLogId])
);
GO

CREATE NONCLUSTERED INDEX [IXQ_UserUsernameHistory_UserId] ON [dbo].[UserUsernameHistory]([UserId] ASC);
GO

INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(6, 'Cambio credenciales', 'Cambio de credenciales de usuario')
GO
