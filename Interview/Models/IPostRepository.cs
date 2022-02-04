using Serilog;
using System.Collections.Generic;

namespace Interview.Models
{
    public interface IPostRepository
    {
        Post GetPost(int id);
        List<Post> GetSuntPosts();
    }
}