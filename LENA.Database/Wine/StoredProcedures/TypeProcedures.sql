CREATE PROCEDURE [Wine].[usp_Type_Create]
    @TypeName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Type] (TypeName, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@TypeName, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Wine].[usp_Type_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Type] WHERE TypeID = @Id;
END
GO

CREATE PROCEDURE [Wine].[usp_Type_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Type] ORDER BY TypeName;
END
GO

CREATE PROCEDURE [Wine].[usp_Type_Update]
    @TypeID INT,
    @TypeName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Type]
    SET TypeName = @TypeName, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE TypeID = @TypeID;
END
GO

CREATE PROCEDURE [Wine].[usp_Type_Delete]
    @TypeID INT
AS
BEGIN
    DELETE FROM [Wine].[Type] WHERE TypeID = @TypeID;
END
GO

CREATE PROCEDURE [Wine].[usp_Type_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Type] WHERE TypeName = @Name;
END
GO
