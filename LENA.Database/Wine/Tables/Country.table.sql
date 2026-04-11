CREATE TABLE [Wine].[Country] (
    [CountryID] INT IDENTITY(1,1) NOT NULL,
    [CountryName] NVARCHAR(100) NOT NULL,
    [ISOCode] NVARCHAR(10) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryID] ASC),
    CONSTRAINT [UQ_Country_ISOCode] UNIQUE ([ISOCode])
);
GO
