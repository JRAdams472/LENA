SET XACT_ABORT ON;
SET NOCOUNT ON;

DECLARE @DefaultUserID INT;

IF NOT EXISTS (SELECT 1 FROM [Identity].[User] WHERE [Provider] = 'google' AND [ExternalSubject] = 'legacy-default')
BEGIN
    INSERT INTO [Identity].[User] ([ExternalSubject], [Provider], [Email], [DisplayName], [IsActive], [LastLoginDate], [CreatedBy], [CreateDate])
    VALUES ('legacy-default', 'google', 'default@lena.local', 'Legacy Default', 1, SYSUTCDATETIME(), 'migration', SYSUTCDATETIME());

    SET @DefaultUserID = CAST(SCOPE_IDENTITY() AS INT);
END
ELSE
BEGIN
    SELECT @DefaultUserID = [UserID] FROM [Identity].[User] WHERE [Provider] = 'google' AND [ExternalSubject] = 'legacy-default';
END

-- Backfill MealPlan.UserID
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[MealPlan].[MealPlan]') AND name = 'UserID')
BEGIN
    ALTER TABLE [MealPlan].[MealPlan] ADD [UserID] INT NULL;
END

UPDATE [MealPlan].[MealPlan] SET [UserID] = @DefaultUserID WHERE [UserID] IS NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[MealPlan].[MealPlan]') AND name = 'UserID' AND is_nullable = 1)
BEGIN
    ALTER TABLE [MealPlan].[MealPlan] ALTER COLUMN [UserID] INT NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'[MealPlan].[MealPlan]') AND name = 'FK_MealPlan_User')
BEGIN
    ALTER TABLE [MealPlan].[MealPlan] WITH NOCHECK ADD CONSTRAINT [FK_MealPlan_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]);
    ALTER TABLE [MealPlan].[MealPlan] CHECK CONSTRAINT [FK_MealPlan_User];
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[MealPlan].[MealPlan]') AND name = 'IX_MealPlan_UserID')
BEGIN
    CREATE INDEX [IX_MealPlan_UserID] ON [MealPlan].[MealPlan] ([UserID], [WeekStartDate]);
END

-- Backfill GroceryList.UserID
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[MealPlan].[GroceryList]') AND name = 'UserID')
BEGIN
    ALTER TABLE [MealPlan].[GroceryList] ADD [UserID] INT NULL;
END

UPDATE [MealPlan].[GroceryList] SET [UserID] = @DefaultUserID WHERE [UserID] IS NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[MealPlan].[GroceryList]') AND name = 'UserID' AND is_nullable = 1)
BEGIN
    ALTER TABLE [MealPlan].[GroceryList] ALTER COLUMN [UserID] INT NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'[MealPlan].[GroceryList]') AND name = 'FK_GroceryList_User')
BEGIN
    ALTER TABLE [MealPlan].[GroceryList] WITH NOCHECK ADD CONSTRAINT [FK_GroceryList_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]);
    ALTER TABLE [MealPlan].[GroceryList] CHECK CONSTRAINT [FK_GroceryList_User];
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[MealPlan].[GroceryList]') AND name = 'IX_GroceryList_UserID')
BEGIN
    CREATE INDEX [IX_GroceryList_UserID] ON [MealPlan].[GroceryList] ([UserID], [GeneratedDate]);
END

-- Verification queries (must return 0 for a clean migration)
SELECT COUNT(*) AS MealPlanNullUserID FROM [MealPlan].[MealPlan] WHERE [UserID] IS NULL;
SELECT COUNT(*) AS GroceryListNullUserID FROM [MealPlan].[GroceryList] WHERE [UserID] IS NULL;
