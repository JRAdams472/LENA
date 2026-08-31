CREATE PROCEDURE [Wine].[usp_Country_GetByISOCode]
    @ISOCode NVARCHAR(10)
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE ISOCode = @ISOCode;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_GetAllActive]
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE IsActive = 1 ORDER BY CountryName;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_Create]
    @CountryName NVARCHAR(100),
    @ISOCode NVARCHAR(10),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Country] (CountryName, ISOCode, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@CountryName, @ISOCode, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Wine].[usp_Country_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE CountryID = @Id;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Country] ORDER BY CountryName;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_Update]
    @CountryID INT,
    @CountryName NVARCHAR(100),
    @ISOCode NVARCHAR(10),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Country]
    SET CountryName = @CountryName, ISOCode = @ISOCode, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE CountryID = @CountryID;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_Delete]
    @CountryID INT
AS
BEGIN
    DELETE FROM [Wine].[Country] WHERE CountryID = @CountryID;
END
GO

CREATE PROCEDURE [Wine].[usp_Country_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE CountryName = @Name;
END
GO
