IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Recipe.Recipe') AND name = 'IsFavorite')
    ALTER TABLE [Recipe].[Recipe] ADD [IsFavorite] BIT DEFAULT 0 NOT NULL;
