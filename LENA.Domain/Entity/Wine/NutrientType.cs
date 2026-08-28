using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Domain.Entity.Wine
{
    public class NutrientType : AuditableEntity
    {
        public int NutrientId { get; set; }
        public string NutrientName { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}