using System;
using System.Collections.Generic;
using System.Linq;

using LENA.Domain.Entity.Inventory;

namespace LENA.API.Contracts.Inventory
{
    public record CreateFlavorProfileRequest
    {
        public int FlavorId { get; init; } = 0;
        public string FlavorName { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Inventory.FlavorProfile ToEntity() => new()
        {
            FlavorId = FlavorId,
            FlavorName = FlavorName,
            IsActive = IsActive,
        };
    }

    public record UpdateFlavorProfileRequest
    {
        public int FlavorId { get; init; } = 0;
        public string FlavorName { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;

        public LENA.Domain.Entity.Inventory.FlavorProfile ToEntity() => new()
        {
            FlavorId = FlavorId,
            FlavorName = FlavorName,
            IsActive = IsActive,
        };
    }

    public record FlavorProfileResponse
    {
        public int FlavorId { get; init; }
        public required string FlavorName { get; init; }
        public bool IsActive { get; init; }

        public static FlavorProfileResponse FromEntity(LENA.Domain.Entity.Inventory.FlavorProfile entity) => new()
        {
            FlavorId = entity.FlavorId,
            FlavorName = entity.FlavorName,
            IsActive = entity.IsActive,
        };
    }
}