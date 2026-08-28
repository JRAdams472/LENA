CREATE TABLE [Inventory].[flavor_profiles] (
    [flavor_id] INT IDENTITY(1,1) NOT NULL,
    [flavor_name] VARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_flavor_profiles] PRIMARY KEY CLUSTERED ([flavor_id] ASC)
);