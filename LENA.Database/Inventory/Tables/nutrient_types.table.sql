CREATE TABLE [Inventory].[nutrient_types] (
    [nutrient_id] INT IDENTITY(1,1) NOT NULL,
    [nutrient_name] VARCHAR(100) NOT NULL UNIQUE,
    [unit_of_measure] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_nutrient_types] PRIMARY KEY CLUSTERED ([nutrient_id] ASC)
);