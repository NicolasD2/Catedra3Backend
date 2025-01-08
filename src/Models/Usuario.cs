using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Models
{
    public class Usuario
    {
        public string Email { get; set; } = string.Empty;
        
        public string Contraseña { get; set; } = string.Empty;
    }
}