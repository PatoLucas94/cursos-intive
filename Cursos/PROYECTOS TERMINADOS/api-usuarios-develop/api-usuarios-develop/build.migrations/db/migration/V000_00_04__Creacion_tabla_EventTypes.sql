CREATE TABLE [dbo].[EventTypes] (
	[EventTypeId] INT          NOT NULL,
	[Name]        VARCHAR(50)  NULL,
	[Description] VARCHAR(100) NULL,
    
	CONSTRAINT [PK_EventTypes] PRIMARY KEY CLUSTERED ([EventTypeId] ASC)
);
GO

INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(1, 'Autenticación', 'Autenticación')
GO
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(2, 'Perfil', 'Perfil')
GO
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(3, 'Registración', 'Registración')
GO
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(4, 'Migración', 'Migración')
GO
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description]) VALUES(5, 'Cambio contraseña', 'Cambio contraseña')
GO