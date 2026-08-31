CREATE PROCEDURE [Inventory].[usp_FoodFlavor_ListAll]
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors];
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetById]
    @Id INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @Id;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByName]
    @Name VARCHAR(50)
AS
BEGIN
    RETURN;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Create]
    @FoodId INT,
    @FlavorId INT,
    @IntensityScore INT
AS
BEGIN
    INSERT INTO [Inventory].[food_flavors] (food_id, flavor_id, intensity_score)
    VALUES (@FoodId, @FlavorId, @IntensityScore);
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Update]
    @FoodId INT,
    @FlavorId INT,
    @IntensityScore INT
AS
BEGIN
    UPDATE [Inventory].[food_flavors]
    SET intensity_score = @IntensityScore
    WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Delete]
    @FoodId INT,
    @FlavorId INT
AS
BEGIN
    DELETE FROM [Inventory].[food_flavors] WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByFoodId]
    @FoodId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @FoodId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByFlavorId]
    @FlavorId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE flavor_id = @FlavorId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByFoodAndFlavorId]
    @FoodId INT,
    @FlavorId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END
GO
