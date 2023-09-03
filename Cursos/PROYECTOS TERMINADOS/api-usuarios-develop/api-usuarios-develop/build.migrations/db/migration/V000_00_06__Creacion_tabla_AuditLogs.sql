CREATE TABLE [dbo].[AuditLogs] (
	[AuditLogId]    INT           IDENTITY (1, 1) NOT NULL,
	[Channel]       VARCHAR(10)   NULL,
	[DateTime]      DATETIME2(3)  NOT NULL,
	[UserId]        INT           NULL,
	[EventTypeId]   INT           NOT NULL,
	[EventResultId] INT           NOT NULL,
	[ExtendedInfo]  NVARCHAR(MAX) NULL,

	CONSTRAINT [PK_AuditLogNew] PRIMARY KEY CLUSTERED ([AuditLogId] ASC),
	CONSTRAINT [FK_AuditLogs->EventTypes] FOREIGN KEY ([EventTypeId]) REFERENCES [dbo].[EventTypes] ([EventTypeId]),
	CONSTRAINT [FK_AuditLogs->EventResults] FOREIGN KEY ([EventResultId]) REFERENCES [dbo].[EventResults] ([EventResultId]),
);
GO