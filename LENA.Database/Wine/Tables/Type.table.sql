CREATE TABLE [Wine].[Type] (
    [TypeID] INT IDENTITY(1,1) NOT NULL,
    [TypeName] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED ([TypeID] ASC),
    CONSTRAINT [UQ_Type_TypeName] UNIQUE ([TypeName])
);
GO
