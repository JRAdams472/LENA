CREATE PROCEDURE [MealPlan].[usp_MealPlan_Delete]
    @MealPlanID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[MealPlan]
    WHERE MealPlanID = @MealPlanID AND UserID = @UserID;

    SELECT @@ROWCOUNT;
END
