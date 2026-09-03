using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Wine;

namespace LENA.API.Contracts.Wine
{
    public record CreateVintageRequest
    {
        public int Year { get; init; } = 0;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Vintage ToEntity() => new()
        {
            Year = Year,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record UpdateVintageRequest
    {
        public int VintageID { get; init; } = 0;
        public int Year { get; init; } = 0;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Vintage ToEntity() => new()
        {
            VintageID = VintageID,
            Year = Year,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record VintageResponse
    {
        public int VintageID { get; init; }
        public int Year { get; init; }
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static VintageResponse FromEntity(LENA.Domain.Entity.Wine.Vintage entity) => new()
        {
            VintageID = entity.VintageID,
            Year = entity.Year,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}