using Serilog;
using System.Collections.Generic;

namespace Interview.Models
{
    public interface IPostRepository
    {
        Post GetPost(int id, bool shoudRefresh = false);
        List<Post> GetSuntPosts(bool shouldRefresh = false);
    }
}