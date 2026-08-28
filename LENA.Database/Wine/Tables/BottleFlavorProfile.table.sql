CREATE TABLE [Wine].[BottleFlavorProfile] (
    [BottleID] INT NOT NULL,
    [FlavorProfileID] INT NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_BottleFlavorProfile] PRIMARY KEY CLUSTERED ([BottleID] ASC, [FlavorProfileID] ASC),
    CONSTRAINT [FK_BottleFlavorProfile_Bottle] FOREIGN KEY ([BottleID]) REFERENCES [Wine].[Bottle] ([BottleID]) ON DELETE CASCADE,
    CONSTRAINT [FK_BottleFlavorProfile_FlavorProfile] FOREIGN KEY ([FlavorProfileID]) REFERENCES [Wine].[FlavorProfile] ([FlavorProfileID]) ON DELETE CASCADE
);
GO