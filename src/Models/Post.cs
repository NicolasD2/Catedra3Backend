using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Models
{
    public class Post
    {
        public string Titulo { get; set; }
        public int Fecha_de_publicacion { get; set; }
        public string Url { get; set; } = string.Empty;

        
    }
}