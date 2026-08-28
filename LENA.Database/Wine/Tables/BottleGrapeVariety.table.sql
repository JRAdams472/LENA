CREATE TABLE [Wine].[BottleGrapeVariety] (
    [BottleID] INT NOT NULL,
    [GrapeVarietyID] INT NOT NULL,
    [Percentage] TINYINT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_BottleGrapeVariety] PRIMARY KEY CLUSTERED ([BottleID] ASC, [GrapeVarietyID] ASC),
    CONSTRAINT [FK_BottleGrapeVariety_Bottle] FOREIGN KEY ([BottleID]) REFERENCES [Wine].[Bottle] ([BottleID]) ON DELETE CASCADE,
    CONSTRAINT [FK_BottleGrapeVariety_GrapeVariety] FOREIGN KEY ([GrapeVarietyID]) REFERENCES [Wine].[GrapeVariety] ([GrapeVarietyID]) ON DELETE CASCADE
);
GO