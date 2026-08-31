CREATE NONCLUSTERED INDEX [IX_RecipeStep_RecipeID_StepNumber]
    ON [Recipe].[RecipeStep] ([RecipeID] ASC, [StepNumber] ASC);
