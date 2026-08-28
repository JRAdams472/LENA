CREATE TABLE [Inventory].[food_flavors] (
    [food_id] INT NOT NULL,
    [flavor_id] INT NOT NULL,
    [intensity_score] INT CHECK ([intensity_score] BETWEEN 1 AND 5),
    CONSTRAINT [PK_food_flavors] PRIMARY KEY CLUSTERED ([food_id] ASC, [flavor_id] ASC),
    CONSTRAINT [FK_food_flavors_food_id] FOREIGN KEY ([food_id]) REFERENCES [Inventory].[Item] ([ItemID]) ON DELETE CASCADE,
    CONSTRAINT [FK_food_flavors_flavor_id] FOREIGN KEY ([flavor_id]) REFERENCES [Inventory].[flavor_profiles] ([flavor_id])
);