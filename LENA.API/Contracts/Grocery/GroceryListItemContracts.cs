using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Grocery;

namespace LENA.API.Contracts.Grocery
{
    public record CreateGroceryListItemRequest
    {
        public int? ItemID { get; init; } = null;
        public string? ItemName { get; init; } = null;
        public string? ManualItemName { get; init; } = null;
        public decimal QuantityNeeded { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public string Source { get; init; } = string.Empty;
        public bool IsChecked { get; init; } = false;

        public LENA.Domain.Entity.Grocery.GroceryListItem ToEntity() => new()
        {
            ItemID = ItemID,
            ItemName = ItemName,
            ManualItemName = ManualItemName,
            QuantityNeeded = QuantityNeeded,
            UnitOfMeasure = UnitOfMeasure,
            Source = Source,
            IsChecked = IsChecked,
        };
    }

    public record UpdateGroceryListItemRequest
    {
        public int GroceryListItemID { get; init; } = 0;
        public int? ItemID { get; init; } = null;
        public string? ItemName { get; init; } = null;
        public string? ManualItemName { get; init; } = null;
        public decimal QuantityNeeded { get; init; } = 0m;
        public string? UnitOfMeasure { get; init; } = null;
        public string Source { get; init; } = string.Empty;
        public bool IsChecked { get; init; } = false;

        public LENA.Domain.Entity.Grocery.GroceryListItem ToEntity() => new()
        {
            GroceryListItemID = GroceryListItemID,
            ItemID = ItemID,
            ItemName = ItemName,
            ManualItemName = ManualItemName,
            QuantityNeeded = QuantityNeeded,
            UnitOfMeasure = UnitOfMeasure,
            Source = Source,
            IsChecked = IsChecked,
        };
    }

    public record GroceryListItemResponse
    {
        public int GroceryListItemID { get; init; }
        public int GroceryListID { get; init; }
        public int? ItemID { get; init; }
        public string? ItemName { get; init; }
        public string? ManualItemName { get; init; }
        public decimal QuantityNeeded { get; init; }
        public string? UnitOfMeasure { get; init; }
        public required string Source { get; init; }
        public bool IsChecked { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static GroceryListItemResponse FromEntity(LENA.Domain.Entity.Grocery.GroceryListItem entity) => new()
        {
            GroceryListItemID = entity.GroceryListItemID,
            GroceryListID = entity.GroceryListID,
            ItemID = entity.ItemID,
            ItemName = entity.ItemName,
            ManualItemName = entity.ManualItemName,
            QuantityNeeded = entity.QuantityNeeded,
            UnitOfMeasure = entity.UnitOfMeasure,
            Source = entity.Source,
            IsChecked = entity.IsChecked,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}