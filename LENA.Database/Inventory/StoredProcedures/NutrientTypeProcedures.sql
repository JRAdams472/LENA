CREATE PROCEDURE [Inventory].[usp_NutrientType_ListAll]
AS
BEGIN
    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    ORDER BY nutrient_name;
END
GO

CREATE PROCEDURE [Inventory].[usp_NutrientType_GetById]
    @Id INT
AS
BEGIN
    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    WHERE nutrient_id = @Id;
END
GO

CREATE PROCEDURE [Inventory].[usp_NutrientType_GetByName]
    @Name VARCHAR(100)
AS
BEGIN
    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    WHERE nutrient_name = @Name;
END
GO

CREATE PROCEDURE [Inventory].[usp_NutrientType_Create]
    @NutrientName VARCHAR(100),
    @UnitOfMeasure VARCHAR(20)
AS
BEGIN
    INSERT INTO [Inventory].[nutrient_types] (nutrient_name, unit_of_measure)
    VALUES (@NutrientName, @UnitOfMeasure);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Inventory].[usp_NutrientType_Update]
    @NutrientId INT,
    @NutrientName VARCHAR(100),
    @UnitOfMeasure VARCHAR(20)
AS
BEGIN
    UPDATE [Inventory].[nutrient_types]
    SET nutrient_name = @NutrientName,
        unit_of_measure = @UnitOfMeasure
    WHERE nutrient_id = @NutrientId;
END
GO

CREATE PROCEDURE [Inventory].[usp_NutrientType_Delete]
    @NutrientId INT
AS
BEGIN
    DELETE FROM [Inventory].[nutrient_types] WHERE nutrient_id = @NutrientId;
END
GO
