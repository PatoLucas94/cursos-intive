-- se agrega constraint para que las reglas de validacion sean UNICAS
ALTER TABLE [dbo].[ValidationRules] ADD CONSTRAINT [UC_ValidationRules_ValidationRuleName] UNIQUE NONCLUSTERED ([ValidationRuleName]);
GO