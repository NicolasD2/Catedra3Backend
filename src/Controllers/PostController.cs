using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostCatedraApi.src.Interfaces;
using PostCatedraApi.src.Models;
using System.Collections.Generic;
using System.Security.Claims;
using PostCatedraApi.src.Mappers;
namespace PostCatedraApi.src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPost()
        {
            var posts = _postRepository.GetPosts();
            var postDto = posts.Select(PostMapper.PostMap).ToList();
            return Ok(postDto);
        }

        [HttpPost]
        public ActionResult<Post> Create([FromBody] Post post)
        {
            if(post.Titulo.Length<5){
                return BadRequest("El titulo debe tener al menos 5 caracteres");
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.Fecha_de_publicacion = DateTime.UtcNow;
            var createdPost = _postRepository.Add(post, userId);
            _postRepository.Save();

            var createdPostDto = PostMapper.PostMap(createdPost);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPostDto);
        }
    }
}
