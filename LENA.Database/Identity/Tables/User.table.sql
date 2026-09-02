CREATE TABLE [Identity].[User] (
    [UserID]          INT IDENTITY(1,1) NOT NULL,
    [ExternalSubject] NVARCHAR(255)     NOT NULL,
    [Provider]        NVARCHAR(50)      NOT NULL CONSTRAINT [DF_User_Provider] DEFAULT N'google',
    [Email]           NVARCHAR(320)     NOT NULL,
    [DisplayName]     NVARCHAR(200)     NULL,
    [IsActive]        BIT               NOT NULL CONSTRAINT [DF_User_IsActive] DEFAULT 1,
    [LastLoginDate]   DATETIME2         NULL,
    [CreatedBy]       NVARCHAR(100)     NOT NULL,
    [CreateDate]      DATETIME2         NOT NULL,
    [LastUpdatedBy]   NVARCHAR(100)     NULL,
    [LastUpdatedDate] DATETIME2         NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID]),
    CONSTRAINT [UQ_User_Provider_ExternalSubject] UNIQUE ([Provider], [ExternalSubject])
);
GO

CREATE INDEX [IX_User_Email] ON [Identity].[User] ([Email]);
GO
