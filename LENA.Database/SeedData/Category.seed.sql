-- Seed data for Inventory.Category table
SET IDENTITY_INSERT [Inventory].[Category] ON;

INSERT INTO [Inventory].[Category] (
    [CategoryID], 
    [CategoryName], 
    [Description], 
    [IsActive],
    [CreatedBy], 
    [CreateDate], 
    [LastUpdatedBy], 
    [LastUpdatedDate]
) VALUES 
(1, 'Produce', 'Fresh fruits, vegetables', 1, 'System', GETUTCDATE(), NULL, NULL),
(2, 'Dairy', 'Milk, cheese, yogurt', 1, 'System', GETUTCDATE(), NULL, NULL),
(3, 'Grains', 'Rice, pasta, bread, oats', 1, 'System', GETUTCDATE(), NULL, NULL),
(4, 'Meats', 'Beef, pork, chicken, etc.', 1, 'System', GETUTCDATE(), NULL, NULL),
(5, 'Seafood', 'Fish, shellfish', 1, 'System', GETUTCDATE(), NULL, NULL),
(6, 'Condiments', 'Sauces, dips, dressings', 1, 'System', GETUTCDATE(), NULL, NULL),
(7, 'Pantry Staples', 'Flour, sugar, salt, baking powder', 1, 'System', GETUTCDATE(), NULL, NULL),
(8, 'Frozen', 'Frozen vegetables, ice cream, etc.', 1, 'System', GETUTCDATE(), NULL, NULL),
(9, 'Wines', 'Red, white, rose, etc.', 1, 'System', GETUTCDATE(), NULL, NULL),
(10, 'Spirits', 'Liquor, wine, beer', 1, 'System', GETUTCDATE(), NULL, NULL),
(11, 'Oils/Vinegars', 'Olive oil, vinegar, etc.', 1, 'System', GETUTCDATE(), NULL, NULL),
(12, 'Spices', 'Salt, pepper, herbs, spices', 1, 'System', GETUTCDATE(), NULL, NULL),
(13, 'Canned Goods', 'Canned vegetables, fruits, soups', 1, 'System', GETUTCDATE(), NULL, NULL),
(14, 'Bakery', 'Bread, pastries, baked goods', 1, 'System', GETUTCDATE(), NULL, NULL),
(15, 'Other', 'Miscellaneous items', 1, 'System', GETUTCDATE(), NULL, NULL);

SET IDENTITY_INSERT [Inventory].[Category] OFF;
