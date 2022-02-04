using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Serilog;

namespace Interview.Models
{
    public class PostRepository
    {
        public static Dictionary<int, Post> _posts = null;
        
        public static Dictionary<int, Post> Posts
        {
            get
            {
                if (_posts == null)
                    PopulatePostCollection();
                return _posts;
            }
        }

        private static void PopulatePostCollection()
        {
            var client = new HttpClient();

            Console.WriteLine($"About to fill cache with all posts");
            var response = client.GetAsync($"https://jsonplaceholder.typicode.com/posts").Result;

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _posts = response.Content.ReadFromJsonAsync<IEnumerable<Post>>().Result.ToDictionary(r => r.id);

            Console.WriteLine($"Filled cache");
        }
    }
}
