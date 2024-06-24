using System;

namespace Superkatten.Katministratie.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string? message) : base(message)
    {
    }
}
