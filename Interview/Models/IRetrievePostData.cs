using System.Collections.Generic;

namespace Interview.Models
{
    public interface IRetrievePostData
    {
        List<Post> GetAllPosts();
    }
}