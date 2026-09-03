using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class FoodFlavor
    {
        public int FoodId { get; set; }
        public int FlavorId { get; set; }
        public int IntensityScore { get; set; }

        // Navigation properties
        public Item? Item { get; set; }
        public FlavorProfile? FlavorProfile { get; set; }
    }
}