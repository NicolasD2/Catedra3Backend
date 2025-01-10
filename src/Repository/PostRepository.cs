using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostCatedraApi.src.Data;
using PostCatedraApi.src.Interfaces;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Repository
{
    public class PostRepository: IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context){
            _context = context;
        }

        public IEnumerable<Post> GetPosts(){
            return _context.Posts.ToList();
        }
        public Post Add(Post post, string userId){
            post.UsuarioId = userId;
            _context.Posts.Add(post);
            return post;
        }
        public void Save(){
            _context.SaveChanges();
        }
    }
}