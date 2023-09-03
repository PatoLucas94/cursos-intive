CREATE TABLE [dbo].[Inputs](
	[InputId]       INT             NOT NULL,
	[InputName]     VARCHAR(200)    NOT NULL,
	[Description]   VARCHAR(300)    NULL,

	CONSTRAINT [PK_Inputs] PRIMARY KEY CLUSTERED ([InputId]),
);
GO

INSERT INTO [dbo].[Inputs]([InputId], [InputName], [Description]) VALUES(1, 'Usuario', 'Usuario')
GO
INSERT INTO [dbo].[Inputs]([InputId], [InputName], [Description]) VALUES(2, 'Clave', 'Clave')
GO
INSERT INTO [dbo].[Inputs]([InputId], [InputName], [Description]) VALUES(3, 'Usuario/Clave', 'Usuario/Clave')
GO