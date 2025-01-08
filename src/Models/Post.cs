using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Models
{
    public class Post
    {
        public string Titulo { get; set; } = string.Empty;
        public DateTime Fecha_de_publicacion { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } 

    }
}