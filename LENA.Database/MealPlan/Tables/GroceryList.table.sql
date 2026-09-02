CREATE TABLE [MealPlan].[GroceryList] (
    [GroceryListID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [MealPlanID] INT NULL,
    [GeneratedDate] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_GroceryList] PRIMARY KEY CLUSTERED ([GroceryListID] ASC),
    CONSTRAINT [FK_GroceryList_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]),
    CONSTRAINT [FK_GroceryList_MealPlan] FOREIGN KEY ([MealPlanID]) REFERENCES [MealPlan].[MealPlan] ([MealPlanID]) ON DELETE SET NULL
);
GO

CREATE INDEX [IX_GroceryList_UserID] ON [MealPlan].[GroceryList] ([UserID], [GeneratedDate]);
GO
