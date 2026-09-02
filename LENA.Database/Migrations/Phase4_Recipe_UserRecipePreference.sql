SET XACT_ABORT ON;
SET NOCOUNT ON;

-- Create per-user recipe preference table
IF OBJECT_ID(N'[Recipe].[UserRecipePreference]', N'U') IS NULL
BEGIN
    CREATE TABLE [Recipe].[UserRecipePreference] (
        [UserID] INT NOT NULL,
        [RecipeID] INT NOT NULL,
        [IsFavorite] BIT NOT NULL CONSTRAINT [DF_UserRecipePreference_IsFavorite] DEFAULT 0,
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreateDate] DATETIME2 NOT NULL,
        [LastUpdatedBy] NVARCHAR(100) NULL,
        [LastUpdatedDate] DATETIME2 NULL,
        CONSTRAINT [PK_UserRecipePreference] PRIMARY KEY CLUSTERED ([UserID], [RecipeID]),
        CONSTRAINT [FK_UserRecipePreference_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]),
        CONSTRAINT [FK_UserRecipePreference_Recipe] FOREIGN KEY ([RecipeID]) REFERENCES [Recipe].[Recipe] ([RecipeID]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_UserRecipePreference_UserID] ON [Recipe].[UserRecipePreference] ([UserID], [IsFavorite]);
END
GO

IF OBJECT_ID('tempdb..#DefaultUserID') IS NOT NULL DROP TABLE #DefaultUserID;
GO

SELECT [UserID] INTO #DefaultUserID FROM [Identity].[User] WHERE [Provider] = 'google' AND [ExternalSubject] = 'legacy-default';
GO

-- Backfill existing recipe favorites for the legacy default user
IF (SELECT COUNT(*) FROM #DefaultUserID) > 0 AND COL_LENGTH(N'[Recipe].[Recipe]', N'IsFavorite') IS NOT NULL
BEGIN
    EXEC('
    INSERT INTO [Recipe].[UserRecipePreference] ([UserID], [RecipeID], [IsFavorite], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate])
    SELECT (SELECT [UserID] FROM #DefaultUserID), [RecipeID], 1, ''migration'', SYSUTCDATETIME(), ''migration'', SYSUTCDATETIME()
    FROM [Recipe].[Recipe]
    WHERE [IsFavorite] = 1
      AND NOT EXISTS (
          SELECT 1 FROM [Recipe].[UserRecipePreference]
          WHERE [UserID] = (SELECT [UserID] FROM #DefaultUserID) AND [RecipeID] = [Recipe].[RecipeID]
      );
    ');
END
GO

-- Drop the now-extracted IsFavorite column
IF COL_LENGTH(N'[Recipe].[Recipe]', N'IsFavorite') IS NOT NULL
BEGIN
    DECLARE @ConstraintName NVARCHAR(255) = (
        SELECT dc.[name]
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE OBJECT_NAME(dc.parent_object_id) = 'Recipe'
          AND OBJECT_SCHEMA_NAME(dc.parent_object_id) = 'Recipe'
          AND c.name = 'IsFavorite'
    );

    IF @ConstraintName IS NOT NULL
        EXEC('ALTER TABLE [Recipe].[Recipe] DROP CONSTRAINT [' + @ConstraintName + ']');

    EXEC('ALTER TABLE [Recipe].[Recipe] DROP COLUMN [IsFavorite]');
END
