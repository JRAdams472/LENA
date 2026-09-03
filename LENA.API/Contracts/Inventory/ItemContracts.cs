using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Inventory;

namespace LENA.API.Contracts.Inventory
{
    public record CreateItemRequest
    {
        public string Name { get; init; } = string.Empty;
        public string? Brand { get; init; } = null;
        public int? BrandID { get; init; } = null;
        public string? UPC12 { get; init; } = null;
        public string? UPC14 { get; init; } = null;
        public int CategoryID { get; init; } = 0;
        public string Unit { get; init; } = string.Empty;
        public decimal CurrentQuantity { get; init; } = 0m;
        public decimal? MinQuantity { get; init; } = null;
        public DateTime? PurchaseDate { get; init; } = null;
        public DateTime? ExpiryDate { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Inventory.Item ToEntity() => new()
        {
            Name = Name,
            Brand = Brand,
            BrandID = BrandID,
            UPC12 = UPC12,
            UPC14 = UPC14,
            CategoryID = CategoryID,
            Unit = Unit,
            CurrentQuantity = CurrentQuantity,
            MinQuantity = MinQuantity,
            PurchaseDate = PurchaseDate,
            ExpiryDate = ExpiryDate,
            Notes = Notes,
            IsFavorite = IsFavorite,
        };
    }

    public record UpdateItemRequest
    {
        public int ItemID { get; init; } = 0;
        public string Name { get; init; } = string.Empty;
        public string? Brand { get; init; } = null;
        public int? BrandID { get; init; } = null;
        public string? UPC12 { get; init; } = null;
        public string? UPC14 { get; init; } = null;
        public int CategoryID { get; init; } = 0;
        public string Unit { get; init; } = string.Empty;
        public decimal CurrentQuantity { get; init; } = 0m;
        public decimal? MinQuantity { get; init; } = null;
        public DateTime? PurchaseDate { get; init; } = null;
        public DateTime? ExpiryDate { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Inventory.Item ToEntity() => new()
        {
            ItemID = ItemID,
            Name = Name,
            Brand = Brand,
            BrandID = BrandID,
            UPC12 = UPC12,
            UPC14 = UPC14,
            CategoryID = CategoryID,
            Unit = Unit,
            CurrentQuantity = CurrentQuantity,
            MinQuantity = MinQuantity,
            PurchaseDate = PurchaseDate,
            ExpiryDate = ExpiryDate,
            Notes = Notes,
            IsFavorite = IsFavorite,
        };
    }

    public record ItemResponse
    {
        public int ItemID { get; init; }
        public required string Name { get; init; }
        public string? Brand { get; init; }
        public int? BrandID { get; init; }
        public string? UPC12 { get; init; }
        public string? UPC14 { get; init; }
        public int CategoryID { get; init; }
        public required string Unit { get; init; }
        public decimal CurrentQuantity { get; init; }
        public decimal? MinQuantity { get; init; }
        public DateTime? PurchaseDate { get; init; }
        public DateTime? ExpiryDate { get; init; }
        public string? Notes { get; init; }
        public bool IsFavorite { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static ItemResponse FromEntity(LENA.Domain.Entity.Inventory.Item entity) => new()
        {
            ItemID = entity.ItemID,
            Name = entity.Name,
            Brand = entity.Brand,
            BrandID = entity.BrandID,
            UPC12 = entity.UPC12,
            UPC14 = entity.UPC14,
            CategoryID = entity.CategoryID,
            Unit = entity.Unit,
            CurrentQuantity = entity.CurrentQuantity,
            MinQuantity = entity.MinQuantity,
            PurchaseDate = entity.PurchaseDate,
            ExpiryDate = entity.ExpiryDate,
            Notes = entity.Notes,
            IsFavorite = entity.IsFavorite,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}