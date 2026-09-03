namespace LENA.Application.Contracts.Auditing
{
    public interface ICurrentUserService
    {
        string UserName { get; }

        int UserID { get; }

        string? ExternalSubject { get; }
    }
}