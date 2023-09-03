UPDATE [dbo].[ValidationRules]
SET [RegularExpression] = '/^(?=.*[A-Za-z0-9\-\.\_]+)([A-Za-z0-9\.\-\_]+)$/'
WHERE ValidationRuleName = 'LetrasYNumerosYCaracteresEspecialesLimitados';
GO