CREATE NONCLUSTERED INDEX [IX_Bottle_CountryID_BottleNumber]
    ON [Wine].[Bottle] ([CountryID] ASC, [BottleNumber] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Bottle_RegionID_BottleNumber]
    ON [Wine].[Bottle] ([RegionID] ASC, [BottleNumber] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Bottle_TypeID_BottleNumber]
    ON [Wine].[Bottle] ([TypeID] ASC, [BottleNumber] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Bottle_VintageYear_BottleNumber]
    ON [Wine].[Bottle] ([VintageYear] ASC, [BottleNumber] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Bottle_IsFavorite_BottleNumber]
    ON [Wine].[Bottle] ([IsFavorite] ASC, [BottleNumber] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Bottle_Vineyard]
    ON [Wine].[Bottle] ([Vineyard] ASC);
