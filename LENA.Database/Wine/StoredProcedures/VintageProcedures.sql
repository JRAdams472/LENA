CREATE PROCEDURE [Wine].[usp_Vintage_Create]
    @Year INT,
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Vintage] (Year, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@Year, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE VintageID = @Id;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] ORDER BY Year;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_Update]
    @VintageID INT,
    @Year INT,
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Vintage]
    SET Year = @Year, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE VintageID = @VintageID;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_Delete]
    @VintageID INT
AS
BEGIN
    DELETE FROM [Wine].[Vintage] WHERE VintageID = @VintageID;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    RETURN;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_GetByYear]
    @Year INT
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE Year = @Year;
END
GO

CREATE PROCEDURE [Wine].[usp_Vintage_GetAllActive]
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE IsActive = 1 ORDER BY Year;
END
GO
