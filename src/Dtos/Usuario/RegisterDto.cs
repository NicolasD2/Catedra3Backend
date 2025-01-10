using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Dtos.Usuario
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email {get; set;} = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage ="La contraseña debe tener minimo 6 caracteres", MinimumLength = 6)]
        public string Password{get; set;} = string.Empty;
    }
}