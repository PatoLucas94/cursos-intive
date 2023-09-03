-- se elimina constraint actual con el campo que es varchar
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [UC_Users_DocumentFields]
GO

-- se cambia el tipo de dato del campo DocumentCountryId
ALTER TABLE [dbo].[Users] ALTER COLUMN [DocumentCountryId] INT
GO

-- se crea nuevamente el indice, pero ahora con el campo DocumentCountryId de tipo INT
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UC_Users_DocumentFields] UNIQUE ([DocumentCountryId], [DocumentTypeId], [DocumentNumber])
GO