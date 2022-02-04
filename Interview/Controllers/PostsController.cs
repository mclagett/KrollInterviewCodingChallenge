using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Interview.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Interview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private ILogger logger = null;
        private IPostRepository postRepository = null;

        public PostsController(ILogger logger, IPostRepository postRepository)
        {
            this.logger = logger;
            this.postRepository = postRepository;
        }

        [HttpGet]
        public Post Get(int id)
        {
            logger.Information("Controller getting single Post from repository");
            return postRepository.GetPost(id);
        }

        [HttpGet]
        [Route("MakeReport")]
        public IEnumerable<Post> AllWithSunt()
        {
            logger.Information("Controller getting Sunt report from repository");
            var suntPosts = postRepository.GetSuntPosts();
            return suntPosts;
        }
    }
}
