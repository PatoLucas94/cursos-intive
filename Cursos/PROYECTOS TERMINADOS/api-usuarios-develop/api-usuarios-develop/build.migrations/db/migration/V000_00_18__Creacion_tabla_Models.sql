CREATE TABLE [dbo].[Models](
	[ModelId]       INT	            NOT NULL,
	[ModelName]     VARCHAR(200)    NOT NULL,
	[Description]   VARCHAR(300)    NULL,

	CONSTRAINT [PK_Models] PRIMARY KEY CLUSTERED ([ModelId])
);
GO

INSERT INTO [dbo].[Models]([ModelId], [ModelName], [Description]) VALUES(1, 'HBI', 'Home Banking Individuo')
GO
INSERT INTO [dbo].[Models]([ModelId], [ModelName], [Description]) VALUES(2, 'HBE', 'Home Banking Empresa')
GO
INSERT INTO [dbo].[Models]([ModelId], [ModelName], [Description]) VALUES(3, 'BUU', 'Base Única de Usuarios')
GO