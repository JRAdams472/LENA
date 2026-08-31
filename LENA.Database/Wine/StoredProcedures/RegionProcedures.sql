CREATE PROCEDURE [Wine].[usp_Region_GetAllByCountryId]
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE CountryID = @CountryId ORDER BY RegionName;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_GetByNameAndCountryId]
    @Name NVARCHAR(100),
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionName = @Name AND CountryID = @CountryId;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_Create]
    @RegionName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CountryID INT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Region] (RegionName, Description, IsActive, CountryID, CreatedBy, CreateDate)
    VALUES (@RegionName, @Description, @IsActive, @CountryID, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Wine].[usp_Region_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionID = @Id;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Region] ORDER BY RegionName;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_Update]
    @RegionID INT,
    @RegionName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CountryID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Region]
    SET RegionName = @RegionName, Description = @Description, IsActive = @IsActive, CountryID = @CountryID,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE RegionID = @RegionID;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_Delete]
    @RegionID INT
AS
BEGIN
    DELETE FROM [Wine].[Region] WHERE RegionID = @RegionID;
END
GO

CREATE PROCEDURE [Wine].[usp_Region_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionName = @Name;
END
GO
