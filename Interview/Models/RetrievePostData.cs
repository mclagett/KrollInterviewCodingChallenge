using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Interview.Models
{
    public class RetrievePostData : IRetrievePostData
    {
        public List<Post> GetAllPosts()
        {
            List<Post> posts;
            var client = new HttpClient();

            var response = client.GetAsync($"https://jsonplaceholder.typicode.com/posts").Result;

            response.EnsureSuccessStatusCode();
            posts = response.Content.ReadFromJsonAsync<IEnumerable<Post>>().Result.ToList();

            return posts;
        }
    }
}
