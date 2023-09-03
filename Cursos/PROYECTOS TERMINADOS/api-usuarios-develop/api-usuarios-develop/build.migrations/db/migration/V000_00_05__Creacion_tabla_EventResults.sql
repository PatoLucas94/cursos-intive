CREATE TABLE [dbo].[EventResults] (
	[EventResultId] INT          NOT NULL,
	[Description]   VARCHAR(100) NOT NULL,
    
	CONSTRAINT [PK_EventResults] PRIMARY KEY CLUSTERED ([EventResultId] ASC)
);
GO

INSERT INTO [dbo].[EventResults]([EventResultId], [Description]) VALUES(1, 'Ok')
GO
INSERT INTO [dbo].[EventResults]([EventResultId], [Description]) VALUES(2, 'Error')
GO
INSERT INTO [dbo].[EventResults]([EventResultId], [Description]) VALUES(3, 'Exception')
GO
INSERT INTO [dbo].[EventResults]([EventResultId], [Description]) VALUES(4, 'Business Validation Error')
GO