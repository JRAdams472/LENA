CREATE TABLE [Wine].[Bottle] (
    [BottleID] INT IDENTITY(1,1) NOT NULL,
    [TypeID] INT NOT NULL,
    [CountryID] INT NOT NULL,
    [RegionID] INT NOT NULL,
    [VintageYear] INT NOT NULL,
    [Vineyard] NVARCHAR(200) NULL,
    [ABV] DECIMAL(5,2) NULL,
    [Acidity] TINYINT NULL,
    [TanninLevel] TINYINT NULL,
    [Body] TINYINT NULL,
    [Sweetness] TINYINT NULL,
    [OakIntegration] BIT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Bottle] PRIMARY KEY CLUSTERED ([BottleID] ASC),
    CONSTRAINT [FK_Bottle_Type] FOREIGN KEY ([TypeID]) REFERENCES [Wine].[Type] ([TypeID]),
    CONSTRAINT [FK_Bottle_Country] FOREIGN KEY ([CountryID]) REFERENCES [Wine].[Country] ([CountryID]),
    CONSTRAINT [FK_Bottle_Region] FOREIGN KEY ([RegionID]) REFERENCES [Wine].[Region] ([RegionID])
);
GO
