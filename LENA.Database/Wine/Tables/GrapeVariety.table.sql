CREATE TABLE [Wine].[GrapeVariety] (
    [GrapeVarietyID] INT IDENTITY(1,1) NOT NULL,
    [GrapeVarietyName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_GrapeVariety] PRIMARY KEY CLUSTERED ([GrapeVarietyID] ASC),
    CONSTRAINT [UQ_GrapeVariety_Name] UNIQUE ([GrapeVarietyName])
);
GO