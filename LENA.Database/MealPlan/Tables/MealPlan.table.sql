CREATE TABLE [MealPlan].[MealPlan] (
    [MealPlanID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [PlanName] NVARCHAR(200) NOT NULL,
    [WeekStartDate] DATE NOT NULL,
    [WeekStartDayOfWeek] TINYINT NOT NULL DEFAULT 0,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_MealPlan] PRIMARY KEY CLUSTERED ([MealPlanID] ASC),
    CONSTRAINT [FK_MealPlan_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID])
);
GO

CREATE INDEX [IX_MealPlan_UserID] ON [MealPlan].[MealPlan] ([UserID], [WeekStartDate]);
GO
