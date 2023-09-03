IF NOT EXISTS (SELECT * FROM [EventTypes] WHERE [EventTypeId] = 12)
BEGIN
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description])
VALUES (12, 'Autenticacion Keycloak', 'Autenticacion Keycloak')
END

IF NOT EXISTS (SELECT * FROM [EventTypes] WHERE [EventTypeId] = 13)
BEGIN
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description])
VALUES (13, 'Introspect Keycloak', 'Introspect Keycloak')
END

IF NOT EXISTS (SELECT * FROM [EventTypes] WHERE [EventTypeId] = 14)
BEGIN
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description])
VALUES (14, 'Logout Keycloak', 'Logout Keycloak')
END

IF NOT EXISTS (SELECT * FROM [EventTypes] WHERE [EventTypeId] = 15)
BEGIN
INSERT INTO [dbo].[EventTypes]([EventTypeId], [Name], [Description])
VALUES (15, 'Refresh Access Keycloak', 'Refresh Access Keycloak')
END
