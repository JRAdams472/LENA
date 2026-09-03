using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Inventory;

namespace LENA.API.Contracts.Inventory
{
    public record CreateFoodNutrientRequest
    {
        public int FoodId { get; init; } = 0;
        public int NutrientId { get; init; } = 0;
        public decimal AmountPerServing { get; init; } = 0m;

        public LENA.Domain.Entity.Inventory.FoodNutrient ToEntity() => new()
        {
            FoodId = FoodId,
            NutrientId = NutrientId,
            AmountPerServing = AmountPerServing,
        };
    }

    public record UpdateFoodNutrientRequest
    {
        public int FoodId { get; init; } = 0;
        public int NutrientId { get; init; } = 0;
        public decimal AmountPerServing { get; init; } = 0m;

        public LENA.Domain.Entity.Inventory.FoodNutrient ToEntity() => new()
        {
            FoodId = FoodId,
            NutrientId = NutrientId,
            AmountPerServing = AmountPerServing,
        };
    }

    public record FoodNutrientResponse
    {
        public int FoodId { get; init; }
        public int NutrientId { get; init; }
        public decimal AmountPerServing { get; init; }

        public static FoodNutrientResponse FromEntity(LENA.Domain.Entity.Inventory.FoodNutrient entity) => new()
        {
            FoodId = entity.FoodId,
            NutrientId = entity.NutrientId,
            AmountPerServing = entity.AmountPerServing,
        };
    }
}