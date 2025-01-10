using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<Post>GetPosts();
        Post Add(Post post);
        void Save();
    }
}