using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class FoodNutrient : AuditableEntity
    {
        public int FoodId { get; set; }
        public int NutrientId { get; set; }
        public decimal AmountPerServing { get; set; }

        // Navigation properties
        public NutrientType? NutrientType { get; set; }
    }
}
