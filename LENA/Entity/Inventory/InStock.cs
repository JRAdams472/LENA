using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Inventory
{
    public class InStock : AuditableEntity
    {
        public int StockID { get; set; }
        public int ItemID { get; set; }
        public decimal QuantityOnHand { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Item? Item { get; set; }
    }
}
