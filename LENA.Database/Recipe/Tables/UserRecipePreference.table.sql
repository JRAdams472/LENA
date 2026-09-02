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
GO

CREATE INDEX [IX_UserRecipePreference_UserID] ON [Recipe].[UserRecipePreference] ([UserID], [IsFavorite]);
GO
