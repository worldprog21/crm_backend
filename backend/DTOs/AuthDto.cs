using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

}

public class AuthResponseDto
{
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
}

