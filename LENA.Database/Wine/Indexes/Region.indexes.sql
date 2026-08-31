CREATE NONCLUSTERED INDEX [IX_Region_CountryID]
    ON [Wine].[Region] ([CountryID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Region_RegionName]
    ON [Wine].[Region] ([RegionName] ASC);
