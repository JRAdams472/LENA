CREATE PROCEDURE [Identity].[usp_User_GetByExternalSubject]
    @Provider NVARCHAR(50),
    @ExternalSubject NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [UserID],
           [ExternalSubject],
           [Provider],
           [Email],
           [DisplayName],
           [IsActive],
           [LastLoginDate],
           [CreatedBy],
           [CreateDate],
           [LastUpdatedBy],
           [LastUpdatedDate]
    FROM [Identity].[User]
    WHERE [Provider] = @Provider
      AND [ExternalSubject] = @ExternalSubject;
END
