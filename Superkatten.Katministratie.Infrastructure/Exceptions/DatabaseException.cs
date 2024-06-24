using System;

namespace Superkatten.Katministratie.Infrastructure.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string? message) : base(message)
        {

        }
    }
}
