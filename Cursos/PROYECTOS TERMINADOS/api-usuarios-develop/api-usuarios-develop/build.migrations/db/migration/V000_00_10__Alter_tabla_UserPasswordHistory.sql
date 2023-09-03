ALTER TABLE [dbo].[UserPasswordHistory] DROP CONSTRAINT [FK_UserPasswordHistory->AuditLogs]
GO

ALTER TABLE [dbo].[UserPasswordHistory] ADD CONSTRAINT [FK_UserPasswordHistory->AuditLogs] FOREIGN KEY ([AuditLogId]) REFERENCES [dbo].[AuditLogs] ([AuditLogId])
GO