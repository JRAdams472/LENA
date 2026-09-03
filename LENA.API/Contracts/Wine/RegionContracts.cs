using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Wine;

namespace LENA.API.Contracts.Wine
{
    public record CreateRegionRequest
    {
        public string RegionName { get; init; } = string.Empty;
        public int CountryID { get; init; } = 0;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Region ToEntity() => new()
        {
            RegionName = RegionName,
            CountryID = CountryID,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record UpdateRegionRequest
    {
        public int RegionID { get; init; } = 0;
        public string RegionName { get; init; } = string.Empty;
        public int CountryID { get; init; } = 0;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Region ToEntity() => new()
        {
            RegionID = RegionID,
            RegionName = RegionName,
            CountryID = CountryID,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record RegionResponse
    {
        public int RegionID { get; init; }
        public required string RegionName { get; init; }
        public int CountryID { get; init; }
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static RegionResponse FromEntity(LENA.Domain.Entity.Wine.Region entity) => new()
        {
            RegionID = entity.RegionID,
            RegionName = entity.RegionName,
            CountryID = entity.CountryID,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}