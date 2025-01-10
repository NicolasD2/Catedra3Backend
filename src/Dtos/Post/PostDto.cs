using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCatedraApi.src.Dtos.Post
{
    public class PostDto
    {
        public string Titulo{get; set;}
        public DateTime FechaDePublicacion{get; set;}
        public string UrlImagen{get; set;}
        public string Contenido{get; set;}
        public string CreadorUserName {get; set;}
    }
}