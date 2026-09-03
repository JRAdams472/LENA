using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Inventory;

namespace LENA.API.Contracts.Inventory
{
    public record CreateNutrientTypeRequest
    {
        public int NutrientId { get; init; } = 0;
        public string NutrientName { get; init; } = string.Empty;
        public string UnitOfMeasure { get; init; } = string.Empty;

        public LENA.Domain.Entity.Inventory.NutrientType ToEntity() => new()
        {
            NutrientId = NutrientId,
            NutrientName = NutrientName,
            UnitOfMeasure = UnitOfMeasure,
        };
    }

    public record UpdateNutrientTypeRequest
    {
        public int NutrientId { get; init; } = 0;
        public string NutrientName { get; init; } = string.Empty;
        public string UnitOfMeasure { get; init; } = string.Empty;

        public LENA.Domain.Entity.Inventory.NutrientType ToEntity() => new()
        {
            NutrientId = NutrientId,
            NutrientName = NutrientName,
            UnitOfMeasure = UnitOfMeasure,
        };
    }

    public record NutrientTypeResponse
    {
        public int NutrientId { get; init; }
        public required string NutrientName { get; init; }
        public required string UnitOfMeasure { get; init; }

        public static NutrientTypeResponse FromEntity(LENA.Domain.Entity.Inventory.NutrientType entity) => new()
        {
            NutrientId = entity.NutrientId,
            NutrientName = entity.NutrientName,
            UnitOfMeasure = entity.UnitOfMeasure,
        };
    }
}