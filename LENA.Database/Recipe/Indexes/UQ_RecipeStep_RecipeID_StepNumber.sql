CREATE UNIQUE NONCLUSTERED INDEX [UQ_RecipeStep_RecipeID_StepNumber]
    ON [Recipe].[RecipeStep] ([RecipeID] ASC, [StepNumber] ASC);
