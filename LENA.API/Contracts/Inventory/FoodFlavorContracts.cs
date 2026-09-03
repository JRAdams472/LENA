using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Inventory;

namespace LENA.API.Contracts.Inventory
{
    public record CreateFoodFlavorRequest
    {
        public int FoodId { get; init; } = 0;
        public int FlavorId { get; init; } = 0;
        public int IntensityScore { get; init; } = 0;

        public LENA.Domain.Entity.Inventory.FoodFlavor ToEntity() => new()
        {
            FoodId = FoodId,
            FlavorId = FlavorId,
            IntensityScore = IntensityScore,
        };
    }

    public record UpdateFoodFlavorRequest
    {
        public int FoodId { get; init; } = 0;
        public int FlavorId { get; init; } = 0;
        public int IntensityScore { get; init; } = 0;

        public LENA.Domain.Entity.Inventory.FoodFlavor ToEntity() => new()
        {
            FoodId = FoodId,
            FlavorId = FlavorId,
            IntensityScore = IntensityScore,
        };
    }

    public record FoodFlavorResponse
    {
        public int FoodId { get; init; }
        public int FlavorId { get; init; }
        public int IntensityScore { get; init; }

        public static FoodFlavorResponse FromEntity(LENA.Domain.Entity.Inventory.FoodFlavor entity) => new()
        {
            FoodId = entity.FoodId,
            FlavorId = entity.FlavorId,
            IntensityScore = entity.IntensityScore,
        };
    }
}