using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Wine;

namespace LENA.API.Contracts.Wine
{
    public record CreateCountryRequest
    {
        public string CountryName { get; init; } = string.Empty;
        public string ISOCode { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Country ToEntity() => new()
        {
            CountryName = CountryName,
            ISOCode = ISOCode,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record UpdateCountryRequest
    {
        public int CountryID { get; init; } = 0;
        public string CountryName { get; init; } = string.Empty;
        public string ISOCode { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Country ToEntity() => new()
        {
            CountryID = CountryID,
            CountryName = CountryName,
            ISOCode = ISOCode,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record CountryResponse
    {
        public int CountryID { get; init; }
        public required string CountryName { get; init; }
        public required string ISOCode { get; init; }
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static CountryResponse FromEntity(LENA.Domain.Entity.Wine.Country entity) => new()
        {
            CountryID = entity.CountryID,
            CountryName = entity.CountryName,
            ISOCode = entity.ISOCode,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}