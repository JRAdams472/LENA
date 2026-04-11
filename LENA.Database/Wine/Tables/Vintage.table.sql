CREATE TABLE [Wine].[Vintage] (
    [VintageID] INT IDENTITY(1,1) NOT NULL,
    [Year] INT NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Vintage] PRIMARY KEY CLUSTERED ([VintageID] ASC),
    CONSTRAINT [UQ_Vintage_Year] UNIQUE ([Year])
);
GO
