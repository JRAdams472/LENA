using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Domain.Entity.Wine
{
    public class FlavorProfile : AuditableEntity
    {
        public int FlavorId { get; set; }
        public string FlavorName { get; set; }
        
        // Navigation properties
        public ICollection<FoodFlavor>? FoodFlavors { get; set; }
    }
}