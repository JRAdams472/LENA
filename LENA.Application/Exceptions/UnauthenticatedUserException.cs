namespace LENA.Application.Exceptions
{
    public class UnauthenticatedUserException : Exception
    {
        public UnauthenticatedUserException()
            : base("The current user could not be resolved. Ensure the request is authenticated and the UserResolutionMiddleware has run.")
        {
        }
    }
}
