CREATE TABLE [Wine].[Region] (
    [RegionID] INT IDENTITY(1,1) NOT NULL,
    [RegionName] NVARCHAR(100) NOT NULL,
    [CountryID] INT NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED ([RegionID] ASC),
    CONSTRAINT [FK_Region_Country] FOREIGN KEY ([CountryID]) REFERENCES [Wine].[Country] ([CountryID])
);
GO
