CREATE TABLE [dbo].[ValidationRules](
	[ValidationRuleId]          INT             IDENTITY(1,1) NOT NULL,
	[ValidationRuleName]        VARCHAR (200)   NOT NULL,
	[ValidationRuleText]        VARCHAR (500)   NOT NULL,
	[IsActive]                  BIT             NULL,
	[ActivationDate]            DATETIME        NULL,
	[InactivationDate]          DATETIME        NULL,
	[IsRequired]                BIT             NULL,
	[RegularExpression]         VARCHAR (500)   NOT NULL,
	[ModelId]                   INT             NOT NULL,
	[InputId]                   INT             NOT NULL,
	[CreatedDate]               DATETIME        CONSTRAINT [DF_ValidationRules_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[ValidationRulePriority]    INT             NOT NULL,

	CONSTRAINT [PK_ValidationRules] PRIMARY KEY CLUSTERED ([ValidationRuleId]),
	CONSTRAINT [FK_ValidationRules->Models] FOREIGN KEY ([ModelId]) REFERENCES [dbo].[Models] ([ModelId]),
	CONSTRAINT [FK_ValidationRules->Inputs] FOREIGN KEY ([InputId]) REFERENCES [dbo].[Inputs] ([InputId])
);
GO

INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText], [IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId], [CreatedDate],[ValidationRulePriority])
VALUES('LetrasYNumeros', 'Letras y números.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/', 3, 1, '2021-12-20 12:00:00.000', 5)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText], [IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId], [CreatedDate],[ValidationRulePriority])
VALUES('Entre8y15Caracteres', 'Entre 8 y 15 caracteres.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^[A-Za-z0-9]{8,15}$/i', 3, 1, '2021-12-20 12:00:00.000', 4)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText], [IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId], [CreatedDate],[ValidationRulePriority])
VALUES('NoMasDe3CaracteresRepetidos', 'No más de 3 caracteres repetidos.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i', 3, 1, '2021-12-20 12:00:00.000', 3)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('NoMasDe3CaracteresConsecutivosAscODesc', 'No más de 3 caracteres consecutivos.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba)).+$/i', 3, 1, '2021-12-20 12:00:00.000', 2)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('AlMenos1LetraMayuscula', 'Recomendamos tener al menos una letra mayúscula.', 1, '2021-12-20 12:00:00.000', NULL, 0, '/^(?=.*[A-Z])([A-Za-z0-9]+)$/', 3, 1, '2021-12-20 12:00:00.000', 1)
GO

INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('SoloNumeros', 'Solo números.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?=.*[0-9])([0-9]+)$/', 3, 2, '2021-12-20 12:00:00.000', 4)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('Solo4Caracteres', '4 caracteres.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^[A-Za-z0-9]{4}$/i', 3, 2, '2021-12-20 12:00:00.000', 3)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('NoMasDe3NumerosRepetidos', 'No más de 3 números repetidos.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?!.*?([0-9])\1\1\1).+$/', 3, 2, '2021-12-20 12:00:00.000', 2)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('NoMasDe3NumerosConsecutivosAscODesc', 'No más de 3 números consecutivos.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/', 3, 2, '2021-12-20 12:00:00.000', 1)
GO

INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('Entre8y14Caracteres', 'Debe tener entre 8 y 14 caracteres.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^.{8,14}$/i', 1, 2, '2021-12-20 12:00:00.000', 4)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('AlMenos1LetraMayusculaEspecial', 'Debe contener al menos una letra mayúscula.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?=.*[A-Z]).*$/', 1, 2, '2021-12-20 12:00:00.000', 3)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('AlMenos2Numeros', 'Debe contener al menos dos números.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(.*(\d+.*\d+).*)$/', 1, 2, '2021-12-20 12:00:00.000', 2)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText],[IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId],[CreatedDate],[ValidationRulePriority])
VALUES('LetrasYNumerosYCaracteresEspecialesLimitados', 'Opcionalmente puede utilizar: punto, guión medio o guión bajo.', 1, '2021-12-20 12:00:00.000', NULL, 1, '/^(?=.*[A-Za-z0-9\_\.\_]+)([A-Za-z0-9\.\-\_]+)$/', 1, 2, '2021-12-20 12:00:00.000', 1)
GO