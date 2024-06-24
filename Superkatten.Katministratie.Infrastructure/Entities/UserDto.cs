using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Superkatten.Katministratie.Infrastructure.Entities;

public class UserDto
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string? PasswordHash { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
}
