using System;

namespace Superkatten.Katministratie.Application.Exceptions;

public class AuthorisationException : Exception
{
    public AuthorisationException(string message) : 
        base(message) 
    { 

    }
}
