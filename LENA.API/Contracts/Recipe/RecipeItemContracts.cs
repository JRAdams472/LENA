using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Recipe;

namespace LENA.API.Contracts.Recipe
{
    public record CreateRecipeItemRequest
    {
        public int ItemID { get; init; } = 0;
        public decimal Quantity { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsOptional { get; init; } = false;
        public string? ItemName { get; init; } = null;
        public string? ItemBrand { get; init; } = null;

        public LENA.Domain.Entity.Recipe.RecipeItem ToEntity() => new()
        {
            ItemID = ItemID,
            Quantity = Quantity,
            UnitOfMeasure = UnitOfMeasure,
            Notes = Notes,
            IsOptional = IsOptional,
            ItemName = ItemName,
            ItemBrand = ItemBrand,
        };
    }

    public record UpdateRecipeItemRequest
    {
        public int ItemID { get; init; } = 0;
        public decimal Quantity { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsOptional { get; init; } = false;
        public string? ItemName { get; init; } = null;
        public string? ItemBrand { get; init; } = null;

        public LENA.Domain.Entity.Recipe.RecipeItem ToEntity() => new()
        {
            ItemID = ItemID,
            Quantity = Quantity,
            UnitOfMeasure = UnitOfMeasure,
            Notes = Notes,
            IsOptional = IsOptional,
            ItemName = ItemName,
            ItemBrand = ItemBrand,
        };
    }

    public record RecipeItemResponse
    {
        public int RecipeID { get; init; }
        public int ItemID { get; init; }
        public decimal Quantity { get; init; }
        public string? UnitOfMeasure { get; init; }
        public string? Notes { get; init; }
        public bool IsOptional { get; init; }
        public string? ItemName { get; init; }
        public string? ItemBrand { get; init; }

        public static RecipeItemResponse FromEntity(LENA.Domain.Entity.Recipe.RecipeItem entity) => new()
        {
            RecipeID = entity.RecipeID,
            ItemID = entity.ItemID,
            Quantity = entity.Quantity,
            UnitOfMeasure = entity.UnitOfMeasure,
            Notes = entity.Notes,
            IsOptional = entity.IsOptional,
            ItemName = entity.ItemName,
            ItemBrand = entity.ItemBrand,
        };
    }
}