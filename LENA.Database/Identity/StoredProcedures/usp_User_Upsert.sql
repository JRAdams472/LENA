CREATE PROCEDURE [Identity].[usp_User_Upsert]
    @Provider NVARCHAR(50),
    @ExternalSubject NVARCHAR(255),
    @Email NVARCHAR(320),
    @DisplayName NVARCHAR(200) = NULL,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2,
    @LastUpdatedBy NVARCHAR(100),
    @LastUpdatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    MERGE [Identity].[User] AS target
    USING (VALUES (@Provider, @ExternalSubject)) AS source ([Provider], [ExternalSubject])
    ON target.[Provider] = source.[Provider]
   AND target.[ExternalSubject] = source.[ExternalSubject]
    WHEN MATCHED THEN UPDATE SET
        [Email] = @Email,
        [DisplayName] = @DisplayName,
        [IsActive] = 1,
        [LastLoginDate] = SYSUTCDATETIME(),
        [LastUpdatedBy] = @LastUpdatedBy,
        [LastUpdatedDate] = @LastUpdatedDate
    WHEN NOT MATCHED THEN INSERT ([Provider], [ExternalSubject], [Email], [DisplayName], [IsActive], [LastLoginDate], [CreatedBy], [CreateDate])
    VALUES (@Provider, @ExternalSubject, @Email, @DisplayName, 1, SYSUTCDATETIME(), @CreatedBy, @CreateDate)
    OUTPUT inserted.[UserID],
           inserted.[ExternalSubject],
           inserted.[Provider],
           inserted.[Email],
           inserted.[DisplayName],
           inserted.[IsActive],
           inserted.[LastLoginDate],
           inserted.[CreatedBy],
           inserted.[CreateDate],
           inserted.[LastUpdatedBy],
           inserted.[LastUpdatedDate];
END
