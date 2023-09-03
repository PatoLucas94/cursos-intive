/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.UserPasswordHistory.Creation_Date', N'Tmp_CreationDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.UserPasswordHistory.Tmp_CreationDate', N'CreationDate', 'COLUMN' 
GO
ALTER TABLE dbo.UserPasswordHistory SET (LOCK_ESCALATION = TABLE)
GO
DROP INDEX IXQ_UserPasswordHistory_UserId ON UserPasswordHistory
GO
CREATE NONCLUSTERED INDEX IXQ_UserPasswordHistory_UserId ON UserPasswordHistory([UserId] ASC);
GO
COMMIT
