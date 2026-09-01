CREATE PROCEDURE [MealPlan].[usp_GroceryList_Create]
    @MealPlanID INT = NULL,
    @GeneratedDate DATETIME2,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[GroceryList] (MealPlanID, GeneratedDate, CreatedBy, CreateDate)
    VALUES (@MealPlanID, @GeneratedDate, @CreatedBy, @CreateDate);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
