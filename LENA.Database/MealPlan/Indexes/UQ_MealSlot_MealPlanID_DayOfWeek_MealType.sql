CREATE UNIQUE NONCLUSTERED INDEX [UQ_MealSlot_MealPlanID_DayOfWeek_MealType]
    ON [MealPlan].[MealSlot] ([MealPlanID] ASC, [DayOfWeek] ASC, [MealType] ASC);
GO
