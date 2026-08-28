CREATE TABLE [Wine].[FlavorProfile] (
    [FlavorProfileID] INT IDENTITY(1,1) NOT NULL,
    [FlavorProfileName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_FlavorProfile] PRIMARY KEY CLUSTERED ([FlavorProfileID] ASC),
    CONSTRAINT [UQ_FlavorProfile_Name] UNIQUE ([FlavorProfileName])
);
GO