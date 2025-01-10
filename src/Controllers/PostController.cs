using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostCatedraApi.src.Interfaces;
using PostCatedraApi.src.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PostCatedraApi.src.Mappers;
using PostCatedraApi.src.Dtos.Post;
using PostCatedraApi.src.Repository;
using Microsoft.Extensions.Logging;
using CloudinaryDotNet;

namespace PostCatedraApi.src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;
        private readonly Cloudinary _cloudinary;
        public PostController(IPostRepository postRepository, ILogger<PostController> logger, Cloudinary cloudinary)
        {
            _postRepository = postRepository;
            _logger = logger;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostDto>> GetPost()
        {
            var posts = _postRepository.GetPosts();
            var postDtos = posts.Select(PostMapper.PostMap).ToList(); // Utiliza PostMapper para convertir los posts
            return Ok(postDtos);
        }

        [HttpPost]
        public ActionResult<PostDto> Create([FromBody] PostDto creationDto)
        {
             _logger.LogInformation("Attempting to create post with title: {Title}", creationDto.Titulo);
            if (creationDto.Titulo.Length < 5)
            {
                 _logger.LogWarning("Post creation failed: Title too short.");
                return BadRequest("El título debe tener al menos 5 caracteres.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null){
                _logger.LogWarning("Post creation failed: No user ID found in token.");
                return Unauthorized("No se pudo obtener la identidad del usuario");
            }
            var post = new Post
            {
                Titulo = creationDto.Titulo,
                Contenido = creationDto.Contenido,
                UrlImagen = creationDto.UrlImagen,
                UsuarioId = userId,
                Fecha_de_publicacion = DateTime.UtcNow
            };

            var createdPost = _postRepository.Add(post, userId);
            _postRepository.Save();

            var createdPostDto = PostMapper.PostMap(createdPost);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPostDto);
        }

    }
}
