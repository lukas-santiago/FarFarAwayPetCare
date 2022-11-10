using System.Runtime.Serialization;

namespace Application.Errors;

public abstract class BaseException : Exception
{
    protected BaseException() { }
    protected BaseException(string? message) : base(message) { }
    protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    protected BaseException(string? message, Exception? innerException) : base(message, innerException) { }
}