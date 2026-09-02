using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Identity
{
    public class User : AuditableEntity
    {
        public int UserID { get; set; }

        public required string ExternalSubject { get; set; }

        public required string Provider { get; set; }

        public required string Email { get; set; }

        public string? DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginDate { get; set; }
    }
}
