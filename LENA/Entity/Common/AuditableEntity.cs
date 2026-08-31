using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Common
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

    }
}

