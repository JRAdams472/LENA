CREATE TABLE [Inventory].[food_nutrients] (
    [food_id] INT NOT NULL,
    [nutrient_id] INT NOT NULL,
    [amount_per_serving] NUMERIC(8,3) NOT NULL,
    CONSTRAINT [PK_food_nutrients] PRIMARY KEY CLUSTERED ([food_id] ASC, [nutrient_id] ASC),
    CONSTRAINT [FK_food_nutrients_food_id] FOREIGN KEY ([food_id]) REFERENCES [Inventory].[Item] ([ItemID]) ON DELETE CASCADE,
    CONSTRAINT [FK_food_nutrients_nutrient_id] FOREIGN KEY ([nutrient_id]) REFERENCES [Inventory].[nutrient_types] ([nutrient_id])
);