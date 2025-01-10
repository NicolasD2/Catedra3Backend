using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Dtos.Usuario
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;} = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage ="La contraseña debe tener minimo 6 caracteres", MinimumLength = 6)]
        public string Password{get; set;} = string.Empty;
    }
}