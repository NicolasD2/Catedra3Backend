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
using CloudinaryDotNet.Actions;

namespace PostCatedraApi.src.Controllers
{
    
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
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // Validar si el archivo es nulo o su tamaño es cero
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No se subió ningún archivo");
                return BadRequest("No se subió ningún archivo.");
            }

            // Validar tamaño máximo del archivo (5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
            {
                _logger.LogWarning("El archivo es demasiado grande: {Size} bytes", file.Length);
                return BadRequest("El archivo es demasiado grande. Tamaño máximo permitido: 5 MB.");
            }

            // Validar formatos permitidos (JPG, PNG)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning("Formato de archivo no permitido: {Extension}", fileExtension);
                return BadRequest("Formato de archivo no permitido. Solo se permiten imágenes JPG y PNG.");
            }

            try
            {
                // Configurar parámetros de subida a Cloudinary
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "post-images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    _logger.LogError("Error al subir a Cloudinary: {Error}", uploadResult.Error.Message);
                    return BadRequest(uploadResult.Error.Message);
                }

                // Retornar URL y PublicId de la imagen subida
                return Ok(new { Url = uploadResult.SecureUrl.ToString(), PublicId = uploadResult.PublicId });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado al subir la imagen: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error al procesar la imagen.");
            }
        }

        [Authorize]
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
