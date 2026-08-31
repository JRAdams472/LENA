CREATE NONCLUSTERED INDEX [IX_food_nutrients_nutrient_id]
    ON [Inventory].[food_nutrients] ([nutrient_id] ASC, [food_id] ASC);
