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
        public APIResponse<Post> Get(int id)
        {
            logger.Information("Controller getting single Post from repository");
            var fetchedPost = postRepository.GetPost(id);

            var apiResponse =  new APIResponse<Post>() 
            { Success = (fetchedPost != null) ? true : false,
              Message = (fetchedPost == null) ? "Could not retrieve requested Post" : "",
              Data = fetchedPost
            };

            return apiResponse;
        }

        [HttpGet]
        [Route("MakeReport")]
        public APIResponse<IEnumerable<Post>> AllWithSunt()
        {
            logger.Information("Controller getting Sunt report from repository");
            var suntPosts = postRepository.GetSuntPosts();

            var apiResponse = new APIResponse<IEnumerable<Post>>()
            {
                Success = (suntPosts != null) ? true : false,
                Message = (suntPosts == null) ? "Unable to generate requested report" : "",
                Data = suntPosts
            };

            return apiResponse;
        }
    }
}
