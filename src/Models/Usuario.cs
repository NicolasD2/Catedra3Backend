using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PostCatedraApi.src.Models
{
    public class Usuario : IdentityUser
    {
        public string Email { get; set; } = string.Empty;

        public string Contraseña { get; set; } = string.Empty;
    }
}