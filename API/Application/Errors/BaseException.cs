using System.Runtime.Serialization;

namespace Application.Errors;

public abstract class BaseException : Exception
{
    protected BaseException() { }
    protected BaseException(string? message) : base(message) { }
    protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    protected BaseException(string? message, Exception? innerException) : base(message, innerException) { }
}

public class NotFoundException : BaseException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class AlreadyExistsException : BaseException
{
    public AlreadyExistsException()
    {
    }

    public AlreadyExistsException(string? message) : base(message)
    {
    }

    public AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public AlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
