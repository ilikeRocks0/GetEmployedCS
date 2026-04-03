namespace Back_end.Persistence.Exceptions;

public class ObjectConversionException : InvalidOperationException
{
    public ObjectConversionException() : base() { }
    public ObjectConversionException(string message) : base(message) { }
}