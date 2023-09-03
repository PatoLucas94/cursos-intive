CREATE TABLE [dbo].[Configurations] (
	[ConfigurationId] INT            IDENTITY (1, 1) NOT NULL,
	[Type]            VARCHAR (50)   NOT NULL,
	[Name]            VARCHAR (50)   NOT NULL,
	[Description]     VARCHAR (200)  NULL,
	[Value]           VARCHAR (8000) NULL,
	[IsSecurity]      BIT            NULL,
	[Rol]             VARCHAR (50)   NULL,
	
	CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([ConfigurationId] ASC),
	CONSTRAINT [UC_Configuration_Name] UNIQUE NONCLUSTERED ([Name] ASC),
);
GO

INSERT INTO [dbo].[Configurations]([Type], [Name], [Description], [Value], [IsSecurity], [Rol])
     VALUES
           ('Users'
           ,'ForceChangePasswordDays'
           ,'Cantidad de días que tiene que pasar un usuario para que tenga que cambiar su contraseña.'
           ,'180'
           ,0
           ,'CONFIGURATION')
GO

INSERT INTO [dbo].[Configurations]([Type], [Name], [Description], [Value], [IsSecurity], [Rol])
     VALUES
           ('Users'
           ,'LoginAttemptsNumber'
           ,'Cantidad de intentos de login en front'
           ,'5'
           ,0
           ,'CONFIGURATION')
GO

INSERT INTO [dbo].[Configurations]([Type], [Name], [Description], [Value], [IsSecurity], [Rol])
     VALUES
           ('Users'
           ,'PasswordHistoryNumber'
           ,'Cantidad de contraseñas guardadas en el histórico.'
           ,'12'
           ,0
           ,'CONFIGURATION')
GO
