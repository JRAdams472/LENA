CREATE TABLE [Recipe].[Recipe] (
    [RecipeID] INT IDENTITY(1,1) NOT NULL,
    [RecipeName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Servings] INT NULL,
    [PrepTimeMinutes] INT NULL,
    [CookTimeMinutes] INT NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED ([RecipeID] ASC),
    CONSTRAINT [UQ_Recipe_RecipeName] UNIQUE ([RecipeName])
);
GO
