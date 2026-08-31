CREATE NONCLUSTERED INDEX [IX_food_flavors_flavor_id]
    ON [Inventory].[food_flavors] ([flavor_id] ASC, [food_id] ASC);
