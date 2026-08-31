CREATE TABLE [Recipe].[RecipeStep] (
    [RecipeStepID] INT IDENTITY(1,1) NOT NULL,
    [RecipeID] INT NOT NULL,
    [StepNumber] INT NOT NULL,
    [Instruction] NVARCHAR(MAX) NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_RecipeStep] PRIMARY KEY CLUSTERED ([RecipeStepID] ASC),
    CONSTRAINT [FK_RecipeStep_Recipe] FOREIGN KEY ([RecipeID]) REFERENCES [Recipe].[Recipe] ([RecipeID]) ON DELETE CASCADE
);
GO
