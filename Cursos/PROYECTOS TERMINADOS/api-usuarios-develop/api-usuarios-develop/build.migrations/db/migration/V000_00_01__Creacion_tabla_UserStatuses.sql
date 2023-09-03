CREATE TABLE [dbo].[UserStatuses] (
	[UserStatusId] TINYINT      NOT NULL,
	[Description]  VARCHAR (20) NOT NULL,
	
	CONSTRAINT [PK_StatusTypes] PRIMARY KEY CLUSTERED ([UserStatusId] ASC)
);
GO

INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (1, 'Pending')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (2, 'In Progress')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (3, 'Active')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (4, 'Declined')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (5, 'Blocked')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (6, 'Migrated')
GO
INSERT INTO [dbo].[UserStatuses]([UserStatusId],[Description]) VALUES (7, 'Inactive')
GO