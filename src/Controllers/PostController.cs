using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostCatedraApi.src.Data;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll(){
            return _context.Posts.ToList();
        }
        [HttpPost]
        public ActionResult<Post> Create([FromBody] Post post){
            _context.Posts.Add(post);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new{id = post.Id}, post);
        }
    }
}