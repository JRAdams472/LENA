CREATE TABLE [MealPlan].[MealSlot] (
    [MealSlotID] INT IDENTITY(1,1) NOT NULL,
    [MealPlanID] INT NOT NULL,
    [DayOfWeek] TINYINT NOT NULL,
    [MealType] TINYINT NOT NULL,
    [RecipeID] INT NULL,
    [Servings] DECIMAL(10,2) DEFAULT 1 NOT NULL,
    [ReplacementNote] NVARCHAR(500) NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_MealSlot] PRIMARY KEY CLUSTERED ([MealSlotID] ASC),
    CONSTRAINT [FK_MealSlot_MealPlan] FOREIGN KEY ([MealPlanID]) REFERENCES [MealPlan].[MealPlan] ([MealPlanID]) ON DELETE CASCADE,
    CONSTRAINT [FK_MealSlot_Recipe] FOREIGN KEY ([RecipeID]) REFERENCES [Recipe].[Recipe] ([RecipeID])
);
GO
