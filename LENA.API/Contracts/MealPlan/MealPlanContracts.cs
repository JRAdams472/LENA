using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.MealPlan;

namespace LENA.API.Contracts.MealPlan
{
    public record CreateMealPlanRequest
    {
        public string PlanName { get; init; } = string.Empty;
        public DateTime WeekStartDate { get; init; }
        public byte WeekStartDayOfWeek { get; init; } = 0;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.MealPlan.MealPlan ToEntity() => new()
        {
            PlanName = PlanName,
            WeekStartDate = WeekStartDate,
            WeekStartDayOfWeek = WeekStartDayOfWeek,
            IsActive = IsActive,
        };
    }

    public record UpdateMealPlanRequest
    {
        public int MealPlanID { get; init; } = 0;
        public string PlanName { get; init; } = string.Empty;
        public DateTime WeekStartDate { get; init; }
        public byte WeekStartDayOfWeek { get; init; } = 0;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.MealPlan.MealPlan ToEntity() => new()
        {
            MealPlanID = MealPlanID,
            PlanName = PlanName,
            WeekStartDate = WeekStartDate,
            WeekStartDayOfWeek = WeekStartDayOfWeek,
            IsActive = IsActive,
        };
    }

    public record MealPlanResponse
    {
        public int MealPlanID { get; init; }
        public int UserID { get; init; }
        public required string PlanName { get; init; }
        public DateTime WeekStartDate { get; init; }
        public byte WeekStartDayOfWeek { get; init; }
        public bool IsActive { get; init; }
        public IReadOnlyList<MealSlotResponse>? MealSlots { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static MealPlanResponse FromEntity(LENA.Domain.Entity.MealPlan.MealPlan entity) => new()
        {
            MealPlanID = entity.MealPlanID,
            UserID = entity.UserID,
            PlanName = entity.PlanName,
            WeekStartDate = entity.WeekStartDate,
            WeekStartDayOfWeek = entity.WeekStartDayOfWeek,
            IsActive = entity.IsActive,
            MealSlots = entity.MealSlots?.Select(MealSlotResponse.FromEntity).ToList(),
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}