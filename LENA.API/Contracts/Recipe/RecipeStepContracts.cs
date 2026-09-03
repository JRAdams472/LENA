using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Recipe;

namespace LENA.API.Contracts.Recipe
{
    public record CreateRecipeStepRequest
    {
        public int StepNumber { get; init; } = 0;
        public string Instruction { get; init; } = string.Empty;

        public LENA.Domain.Entity.Recipe.RecipeStep ToEntity() => new()
        {
            StepNumber = StepNumber,
            Instruction = Instruction,
        };
    }

    public record UpdateRecipeStepRequest
    {
        public int RecipeStepID { get; init; } = 0;
        public int StepNumber { get; init; } = 0;
        public string Instruction { get; init; } = string.Empty;

        public LENA.Domain.Entity.Recipe.RecipeStep ToEntity() => new()
        {
            RecipeStepID = RecipeStepID,
            StepNumber = StepNumber,
            Instruction = Instruction,
        };
    }

    public record RecipeStepResponse
    {
        public int RecipeStepID { get; init; }
        public int RecipeID { get; init; }
        public int StepNumber { get; init; }
        public required string Instruction { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static RecipeStepResponse FromEntity(LENA.Domain.Entity.Recipe.RecipeStep entity) => new()
        {
            RecipeStepID = entity.RecipeStepID,
            RecipeID = entity.RecipeID,
            StepNumber = entity.StepNumber,
            Instruction = entity.Instruction,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}