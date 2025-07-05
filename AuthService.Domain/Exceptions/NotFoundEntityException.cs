namespace AuthService.Domain.Exceptions;

public class NotFoundEntityException : Exception
{
    public NotFoundEntityException(string entity, int id) 
        : base($"The entity {entity} with id {id} doesn't exist")
    {
    }
    
    public NotFoundEntityException(string entity, Guid code) 
        : base($"The entity {entity} with code {code} doesn't exist")
    {
    }

    public NotFoundEntityException(string message) : base(message)
    {
    }
}