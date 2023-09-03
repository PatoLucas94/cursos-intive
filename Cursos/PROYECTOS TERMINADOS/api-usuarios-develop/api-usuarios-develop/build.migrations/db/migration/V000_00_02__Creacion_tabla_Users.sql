CREATE TABLE [dbo].[Users] (
	[UserId]             INT           IDENTITY (1, 1) NOT NULL,
	[PersonId]           VARCHAR (20)  NOT NULL,
	[DocumentCountryId]  VARCHAR (10)  NOT NULL,
	[DocumentTypeId]     INT           NOT NULL,
	[DocumentNumber]     VARCHAR (20)  NOT NULL,
	[Username]           VARCHAR (100) NOT NULL,
	[Password]           VARCHAR (100) NOT NULL,
	[UserStatusId]       TINYINT       CONSTRAINT [DF_Users_UserStatus] DEFAULT ((1)) NOT NULL,	
	[LoginAttempts]      INT           CONSTRAINT [DF_Users_LoginAttempts] DEFAULT ((0)) NULL,
	[LastPasswordChange] DATETIME      NULL,    
	[LastLogon]          DATETIME      NULL,
	[CreatedDate]        DATETIME      CONSTRAINT [DF_Users_CreatedDate] DEFAULT (getdate()) NOT NULL,	

	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId]),
	CONSTRAINT [FK_Users->UserStatuses] FOREIGN KEY ([UserStatusId]) REFERENCES [dbo].[UserStatuses] ([UserStatusId]),
	CONSTRAINT [UC_Users_UsernameWithDocNumber] UNIQUE NONCLUSTERED ([DocumentNumber] ASC, [Username] ASC),
	CONSTRAINT [UC_Users_DocumentFields] UNIQUE ([DocumentCountryId], [DocumentTypeId], [DocumentNumber])	
);
GO