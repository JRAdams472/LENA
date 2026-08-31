using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class NutrientType
    {
        public int NutrientId { get; set; }
        public required string NutrientName { get; set; }
        public required string UnitOfMeasure { get; set; }
    }
}
