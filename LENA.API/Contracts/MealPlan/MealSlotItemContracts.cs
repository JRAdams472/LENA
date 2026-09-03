using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.MealPlan;

namespace LENA.API.Contracts.MealPlan
{
    public record CreateMealSlotItemRequest
    {
        public int ItemID { get; init; } = 0;
        public decimal Quantity { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public bool IsFromRecipe { get; init; } = false;

        public LENA.Domain.Entity.MealPlan.MealSlotItem ToEntity() => new()
        {
            ItemID = ItemID,
            Quantity = Quantity,
            UnitOfMeasure = UnitOfMeasure,
            IsFromRecipe = IsFromRecipe,
        };
    }

    public record UpdateMealSlotItemRequest
    {
        public int MealSlotItemID { get; init; } = 0;
        public int ItemID { get; init; } = 0;
        public decimal Quantity { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public bool IsFromRecipe { get; init; } = false;

        public LENA.Domain.Entity.MealPlan.MealSlotItem ToEntity() => new()
        {
            MealSlotItemID = MealSlotItemID,
            ItemID = ItemID,
            Quantity = Quantity,
            UnitOfMeasure = UnitOfMeasure,
            IsFromRecipe = IsFromRecipe,
        };
    }

    public record MealSlotItemResponse
    {
        public int MealSlotItemID { get; init; }
        public int MealSlotID { get; init; }
        public int ItemID { get; init; }
        public decimal Quantity { get; init; }
        public string? UnitOfMeasure { get; init; }
        public bool IsFromRecipe { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static MealSlotItemResponse FromEntity(LENA.Domain.Entity.MealPlan.MealSlotItem entity) => new()
        {
            MealSlotItemID = entity.MealSlotItemID,
            MealSlotID = entity.MealSlotID,
            ItemID = entity.ItemID,
            Quantity = entity.Quantity,
            UnitOfMeasure = entity.UnitOfMeasure,
            IsFromRecipe = entity.IsFromRecipe,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}