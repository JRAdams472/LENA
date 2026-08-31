using LENA.Domain.Entity.Common;

namespace LENA.Application.Contracts.Auditing
{
    public interface IAuditableCommand
    {
        AuditableEntity AuditableEntity { get; }
    }

    public interface ICreateCommand : IAuditableCommand
    {
    }

    public interface IUpdateCommand : IAuditableCommand
    {
    }
}
