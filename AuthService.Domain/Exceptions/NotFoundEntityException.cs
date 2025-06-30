namespace AuthService.Domain.Exceptions;

public class NotFoundEntityException : Exception
{
    public NotFoundEntityException(Guid code) : base($"The entity with code {code} doesn't exist")
    {
    }

    public NotFoundEntityException(string message) : base(message)
    {
    }
}