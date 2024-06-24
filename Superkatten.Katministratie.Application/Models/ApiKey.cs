using System;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.Models
{
    public class ApiKey
    {
        public string key { get; init; } = string.Empty;
        public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
    }
}
