using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.MealPlan;

namespace LENA.API.Contracts.MealPlan
{
    public record CreateMealSlotRequest
    {
        public byte DayOfWeek { get; init; } = 0;
        public byte MealType { get; init; } = 0;
        public int? RecipeID { get; init; } = null;
        public decimal Servings { get; init; } = 1m;
        public string? ReplacementNote { get; init; } = null;

        public LENA.Domain.Entity.MealPlan.MealSlot ToEntity() => new()
        {
            DayOfWeek = DayOfWeek,
            MealType = MealType,
            RecipeID = RecipeID,
            Servings = Servings,
            ReplacementNote = ReplacementNote,
        };
    }

    public record UpdateMealSlotRequest
    {
        public int MealSlotID { get; init; } = 0;
        public byte DayOfWeek { get; init; } = 0;
        public byte MealType { get; init; } = 0;
        public int? RecipeID { get; init; } = null;
        public decimal Servings { get; init; } = 1m;
        public string? ReplacementNote { get; init; } = null;

        public LENA.Domain.Entity.MealPlan.MealSlot ToEntity() => new()
        {
            MealSlotID = MealSlotID,
            DayOfWeek = DayOfWeek,
            MealType = MealType,
            RecipeID = RecipeID,
            Servings = Servings,
            ReplacementNote = ReplacementNote,
        };
    }

    public record MealSlotResponse
    {
        public int MealSlotID { get; init; }
        public int MealPlanID { get; init; }
        public byte DayOfWeek { get; init; }
        public byte MealType { get; init; }
        public int? RecipeID { get; init; }
        public decimal Servings { get; init; }
        public string? ReplacementNote { get; init; }
        public IReadOnlyList<MealSlotItemResponse>? MealSlotItems { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static MealSlotResponse FromEntity(LENA.Domain.Entity.MealPlan.MealSlot entity) => new()
        {
            MealSlotID = entity.MealSlotID,
            MealPlanID = entity.MealPlanID,
            DayOfWeek = entity.DayOfWeek,
            MealType = entity.MealType,
            RecipeID = entity.RecipeID,
            Servings = entity.Servings,
            ReplacementNote = entity.ReplacementNote,
            MealSlotItems = entity.MealSlotItems?.Select(MealSlotItemResponse.FromEntity).ToList(),
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}