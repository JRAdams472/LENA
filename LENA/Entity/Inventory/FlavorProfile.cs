using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class FlavorProfile : AuditableEntity
    {
        public int FlavorId { get; set; }
        public string FlavorName { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<FoodFlavor>? FoodFlavors { get; set; }
    }
}
