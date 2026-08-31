using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class Category : AuditableEntity
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
