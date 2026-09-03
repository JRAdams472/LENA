using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Grocery;

namespace LENA.API.Contracts.Grocery
{
    public record CreateGroceryListRequest
    {
        public int? MealPlanID { get; init; } = null;
        public DateTime GeneratedDate { get; init; }

        public LENA.Domain.Entity.Grocery.GroceryList ToEntity() => new()
        {
            MealPlanID = MealPlanID,
            GeneratedDate = GeneratedDate,
        };
    }

    public record UpdateGroceryListRequest
    {
        public int GroceryListID { get; init; } = 0;
        public int? MealPlanID { get; init; } = null;
        public DateTime GeneratedDate { get; init; }

        public LENA.Domain.Entity.Grocery.GroceryList ToEntity() => new()
        {
            GroceryListID = GroceryListID,
            MealPlanID = MealPlanID,
            GeneratedDate = GeneratedDate,
        };
    }

    public record GroceryListResponse
    {
        public int GroceryListID { get; init; }
        public int UserID { get; init; }
        public int? MealPlanID { get; init; }
        public DateTime GeneratedDate { get; init; }
        public IReadOnlyList<GroceryListItemResponse>? GroceryListItems { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static GroceryListResponse FromEntity(LENA.Domain.Entity.Grocery.GroceryList entity) => new()
        {
            GroceryListID = entity.GroceryListID,
            UserID = entity.UserID,
            MealPlanID = entity.MealPlanID,
            GeneratedDate = entity.GeneratedDate,
            GroceryListItems = entity.GroceryListItems?.Select(GroceryListItemResponse.FromEntity).ToList(),
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}