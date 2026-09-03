using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Wine;

namespace LENA.API.Contracts.Wine
{
    public record CreateBottleRequest
    {
        public int? BottleNumber { get; init; } = null;
        public int TypeID { get; init; } = 0;
        public int CountryID { get; init; } = 0;
        public int RegionID { get; init; } = 0;
        public int VintageYear { get; init; } = 0;
        public string? Vineyard { get; init; } = null;
        public decimal? ABV { get; init; } = null;
        public int? Acidity { get; init; } = null;
        public int? TanninLevel { get; init; } = null;
        public int? Body { get; init; } = null;
        public int? Sweetness { get; init; } = null;
        public bool? OakIntegration { get; init; } = null;
        public string BottleSize { get; init; } = "750ml";
        public int Quantity { get; init; } = 1;
        public DateTime? PurchaseDate { get; init; } = null;
        public decimal? PurchasePrice { get; init; } = null;
        public decimal? StorageTemp { get; init; } = null;
        public string? Location { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Wine.Bottle ToEntity() => new()
        {
            BottleNumber = BottleNumber,
            TypeID = TypeID,
            CountryID = CountryID,
            RegionID = RegionID,
            VintageYear = VintageYear,
            Vineyard = Vineyard,
            ABV = ABV,
            Acidity = Acidity,
            TanninLevel = TanninLevel,
            Body = Body,
            Sweetness = Sweetness,
            OakIntegration = OakIntegration,
            BottleSize = BottleSize,
            Quantity = Quantity,
            PurchaseDate = PurchaseDate,
            PurchasePrice = PurchasePrice,
            StorageTemp = StorageTemp,
            Location = Location,
            Notes = Notes,
            IsFavorite = IsFavorite,
        };
    }

    public record UpdateBottleRequest
    {
        public int BottleID { get; init; } = 0;
        public int? BottleNumber { get; init; } = null;
        public int TypeID { get; init; } = 0;
        public int CountryID { get; init; } = 0;
        public int RegionID { get; init; } = 0;
        public int VintageYear { get; init; } = 0;
        public string? Vineyard { get; init; } = null;
        public decimal? ABV { get; init; } = null;
        public int? Acidity { get; init; } = null;
        public int? TanninLevel { get; init; } = null;
        public int? Body { get; init; } = null;
        public int? Sweetness { get; init; } = null;
        public bool? OakIntegration { get; init; } = null;
        public string BottleSize { get; init; } = "750ml";
        public int Quantity { get; init; } = 1;
        public DateTime? PurchaseDate { get; init; } = null;
        public decimal? PurchasePrice { get; init; } = null;
        public decimal? StorageTemp { get; init; } = null;
        public string? Location { get; init; } = null;
        public string? Notes { get; init; } = null;
        public bool IsFavorite { get; init; }

        public LENA.Domain.Entity.Wine.Bottle ToEntity() => new()
        {
            BottleID = BottleID,
            BottleNumber = BottleNumber,
            TypeID = TypeID,
            CountryID = CountryID,
            RegionID = RegionID,
            VintageYear = VintageYear,
            Vineyard = Vineyard,
            ABV = ABV,
            Acidity = Acidity,
            TanninLevel = TanninLevel,
            Body = Body,
            Sweetness = Sweetness,
            OakIntegration = OakIntegration,
            BottleSize = BottleSize,
            Quantity = Quantity,
            PurchaseDate = PurchaseDate,
            PurchasePrice = PurchasePrice,
            StorageTemp = StorageTemp,
            Location = Location,
            Notes = Notes,
            IsFavorite = IsFavorite,
        };
    }

    public record BottleResponse
    {
        public int BottleID { get; init; }
        public int? BottleNumber { get; init; }
        public int TypeID { get; init; }
        public int CountryID { get; init; }
        public int RegionID { get; init; }
        public int VintageYear { get; init; }
        public string? Vineyard { get; init; }
        public decimal? ABV { get; init; }
        public int? Acidity { get; init; }
        public int? TanninLevel { get; init; }
        public int? Body { get; init; }
        public int? Sweetness { get; init; }
        public bool? OakIntegration { get; init; }
        public required string BottleSize { get; init; }
        public int Quantity { get; init; }
        public DateTime? PurchaseDate { get; init; }
        public decimal? PurchasePrice { get; init; }
        public decimal? StorageTemp { get; init; }
        public string? Location { get; init; }
        public string? Notes { get; init; }
        public bool IsFavorite { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static BottleResponse FromEntity(LENA.Domain.Entity.Wine.Bottle entity) => new()
        {
            BottleID = entity.BottleID,
            BottleNumber = entity.BottleNumber,
            TypeID = entity.TypeID,
            CountryID = entity.CountryID,
            RegionID = entity.RegionID,
            VintageYear = entity.VintageYear,
            Vineyard = entity.Vineyard,
            ABV = entity.ABV,
            Acidity = entity.Acidity,
            TanninLevel = entity.TanninLevel,
            Body = entity.Body,
            Sweetness = entity.Sweetness,
            OakIntegration = entity.OakIntegration,
            BottleSize = entity.BottleSize,
            Quantity = entity.Quantity,
            PurchaseDate = entity.PurchaseDate,
            PurchasePrice = entity.PurchasePrice,
            StorageTemp = entity.StorageTemp,
            Location = entity.Location,
            Notes = entity.Notes,
            IsFavorite = entity.IsFavorite,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}