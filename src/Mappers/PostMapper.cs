using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostCatedraApi.src.Dtos.Post;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Mappers
{
    public class PostMapper
    {
        public static PostDto PostMap(Post post){
            return new PostDto
            {
                Titulo= post.Titulo,
                FechaDePublicacion = post.Fecha_de_publicacion,
                UrlImagen = post.UrlImagen,
                CreadorUserName = post.Usuario.UserName
            };
        }
    }
}