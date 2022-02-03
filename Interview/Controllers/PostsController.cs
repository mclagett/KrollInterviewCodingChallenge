using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers
{
    public class Post
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
    
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private static readonly Dictionary<int, Post> Cache = new();
        private readonly List<Post> _suntCache = new();

        [HttpGet]
        public async Task<Post> Get(int id)
        {
            if (Cache.ContainsKey(id))
            {
                Console.WriteLine($"Retrieving from cache for id: {id}");
                return Cache[id];
            }
            
            var client = new HttpClient();
            
            Console.WriteLine($"About to fill cache with all posts");
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts");
            
            response.EnsureSuccessStatusCode();
            
            var posts = await response.Content.ReadFromJsonAsync<IEnumerable<Post>>();

            foreach (var post in posts)
            {
                Cache.Add(post.id, post);
            }
            
            Console.WriteLine($"Filled cache");
            
            return Cache[id];
        }
        
        // Posts with "sunt" in body or title should be in the report
        // Posts with "eius" in body or title should be in the report
        // Posts with "voluptatibus" or "voluptatem" in the title - and "facilis" or "nihil" in the body should show up LAST in the report
        // We don't want posts from admins, except the first post; The first post should ALWAYS be included in the report, even from admins.  
        // Admins are userIds 1, 8, and 9.
        [HttpGet]
        [Route("MakeReport")]
        public IEnumerable<Post> AllWithSunt()
        {
            List<Post> volupList = new();
            
            if (!Cache.Any())
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
            
                var posts = response.Content.ReadFromJsonAsync<IEnumerable<Post>>().Result;

                foreach (var post in posts)
                {
                    Cache.Add(post.id, post);
                }
                
                Console.WriteLine($"Filled cache");
            }
            
            if (!_suntCache.Any())
            {
                Console.WriteLine($"Filling SuntCache");

                foreach (var postItem in Cache)
                {
                    if (postItem.Value.title.Contains("sunt") || postItem.Value.body.Contains("sunt"))
                    {
                        _suntCache.Add(postItem.Value);
                        
                        if (postItem.Value.userId == 9 || postItem.Value.userId == 1) 
                        {
                            if(postItem.Value.id != 1)
                                _suntCache.Remove(postItem.Value);
                        }
                    }
                    
                    if (postItem.Value.title.Contains("eius") || postItem.Value.body.Contains("eius"))
                    {
                        _suntCache.Add(postItem.Value);
                        
                        if (postItem.Value.userId == 9 || postItem.Value.userId == 1) 
                        {
                            if(postItem.Value.id != 1)
                                _suntCache.Remove(postItem.Value);
                        }
                    }
                    
                    if (postItem.Value.title.Contains("voluptatibus") || postItem.Value.body.Contains("voluptatem"))
                    {
                        if(postItem.Value.body.Contains("facilis") || postItem.Value.body.Contains("nihil"))
                            volupList.Add(postItem.Value);
                    }
                }
            }

            _suntCache.AddRange(volupList);
            
            return _suntCache;
        }
    }
}
