using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Recipe;

namespace LENA.API.Contracts.Recipe
{
    public record CreateRecipeRequest
    {
        public string RecipeName { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public int? Servings { get; init; } = null;
        public int? PrepTimeMinutes { get; init; } = null;
        public int? CookTimeMinutes { get; init; } = null;
        public bool IsActive { get; init; } = true;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Recipe.Recipe ToEntity() => new()
        {
            RecipeName = RecipeName,
            Description = Description,
            Servings = Servings,
            PrepTimeMinutes = PrepTimeMinutes,
            CookTimeMinutes = CookTimeMinutes,
            IsActive = IsActive,
            IsFavorite = IsFavorite,
        };
    }

    public record UpdateRecipeRequest
    {
        public int RecipeID { get; init; } = 0;
        public string RecipeName { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public int? Servings { get; init; } = null;
        public int? PrepTimeMinutes { get; init; } = null;
        public int? CookTimeMinutes { get; init; } = null;
        public bool IsActive { get; init; } = true;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Recipe.Recipe ToEntity() => new()
        {
            RecipeID = RecipeID,
            RecipeName = RecipeName,
            Description = Description,
            Servings = Servings,
            PrepTimeMinutes = PrepTimeMinutes,
            CookTimeMinutes = CookTimeMinutes,
            IsActive = IsActive,
            IsFavorite = IsFavorite,
        };
    }

    public record RecipeResponse
    {
        public int RecipeID { get; init; }
        public required string RecipeName { get; init; }
        public string? Description { get; init; }
        public int? Servings { get; init; }
        public int? PrepTimeMinutes { get; init; }
        public int? CookTimeMinutes { get; init; }
        public bool IsActive { get; init; }
        public bool IsFavorite { get; init; }
        public IReadOnlyList<RecipeItemResponse>? RecipeItems { get; init; }
        public IReadOnlyList<RecipeStepResponse>? RecipeSteps { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static RecipeResponse FromEntity(LENA.Domain.Entity.Recipe.Recipe entity) => new()
        {
            RecipeID = entity.RecipeID,
            RecipeName = entity.RecipeName,
            Description = entity.Description,
            Servings = entity.Servings,
            PrepTimeMinutes = entity.PrepTimeMinutes,
            CookTimeMinutes = entity.CookTimeMinutes,
            IsActive = entity.IsActive,
            IsFavorite = entity.IsFavorite,
            RecipeItems = entity.RecipeItems?.Select(RecipeItemResponse.FromEntity).ToList(),
            RecipeSteps = entity.RecipeSteps?.Select(RecipeStepResponse.FromEntity).ToList(),
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}