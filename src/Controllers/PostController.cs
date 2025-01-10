using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostCatedraApi.src.Interfaces;
using PostCatedraApi.src.Models;
using System.Collections.Generic;

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
            return Ok(_postRepository.GetPosts());
        }

        [HttpPost]
        public ActionResult<Post> Create([FromBody] Post post)
        {
            var createdPost = _postRepository.Add(post);
            _postRepository.Save();
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }
    }
}
