INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText], [IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId], [CreatedDate],[ValidationRulePriority])
VALUES('Entre8y15CaracteresHBI', 'Entre 8 y 15 caracteres.', 1, '2021-03-07 12:00:00.000', NULL, 1, '/^.{8,15}$/i', 1, 1, '2021-03-07 12:00:00.000', 2)
GO
INSERT INTO [dbo].[ValidationRules]([ValidationRuleName], [ValidationRuleText], [IsActive], [ActivationDate], [InactivationDate], [IsRequired], [RegularExpression], [ModelId], [InputId], [CreatedDate],[ValidationRulePriority])
VALUES('LetrasYNumerosHBI', 'Letras y números.', 1, '2021-03-07 12:00:00.000', NULL, 1, '/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/', 1, 1, '2021-03-07 12:00:00.000', 1)
GO