using System;

namespace Superkatten.Katministratie.Application.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(string? message):base(message)
    {

    }
}
