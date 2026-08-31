using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class NutrientType : AuditableEntity
    {
        public int NutrientId { get; set; }
        public string NutrientName { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
