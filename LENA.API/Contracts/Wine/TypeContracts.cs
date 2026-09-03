using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Wine;

namespace LENA.API.Contracts.Wine
{
    public record CreateTypeRequest
    {
        public string TypeName { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Type ToEntity() => new()
        {
            TypeName = TypeName,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record UpdateTypeRequest
    {
        public int TypeID { get; init; } = 0;
        public string TypeName { get; init; } = string.Empty;
        public string? Description { get; init; } = null;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Wine.Type ToEntity() => new()
        {
            TypeID = TypeID,
            TypeName = TypeName,
            Description = Description,
            IsActive = IsActive,
        };
    }

    public record TypeResponse
    {
        public int TypeID { get; init; }
        public required string TypeName { get; init; }
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public required string CreatedBy { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public string? LastUpdatedBy { get; init; }

        public static TypeResponse FromEntity(LENA.Domain.Entity.Wine.Type entity) => new()
        {
            TypeID = entity.TypeID,
            TypeName = entity.TypeName,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            LastUpdatedDate = entity.LastUpdatedDate,
            CreateDate = entity.CreateDate,
            LastUpdatedBy = entity.LastUpdatedBy,
        };
    }
}