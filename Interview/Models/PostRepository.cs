using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Interview.Models
{
    public class PostRepository : IPostRepository
    {
        public static Dictionary<int, Post> _posts = new Dictionary<int, Post>();
        public static List<Post> _suntPosts = new List<Post>();

        private static ILogger _logger = null;
        private static IRetrievePostData _dataRetriever = null;

        // have to use a Configure method because can't use static constructor for DI
        // this is called in Startup.Configure
        public static void Configure(ILogger logger, IRetrievePostData dataRetriever)
        { 
            _logger = logger;
            _dataRetriever = dataRetriever;
        }

        public static Dictionary<int, Post> Posts
        {
            get
            {
                if (_posts.Count == 0)
                    PopulatePostCollection();
                return _posts;
            }
            internal set
            {
                _posts = value;
            }
        }

        public static List<Post> SuntPosts
        {
            get
            {
                if (_suntPosts.Count == 0)
                    PopulateSuntPostList();
                return _suntPosts;
            }
            internal set
            {
                _suntPosts = value;
            }
        }

        private static void PopulateSuntPostList()
        {
            _suntPosts = new List<Post>();

            // Posts with "sunt" in body or title should be in the report
            // Posts with "eius" in body or title should be in the report
            // Posts with "voluptatibus" or "voluptatem" in the title - and "facilis" or "nihil" in the body should show up LAST in the report
            // We don't want posts from admins, except the first post; The first post should ALWAYS be included in the report, even from admins.  
            // Admins are userIds 1, 8, and 9.

            try
            {
                var adminIds = new int[] { 1, 8, 9 };

                Func<Post, bool> predicate = p =>
                     !adminIds.Contains(p.userId) &&
                     ((p.title.Contains("sunt") || p.body.Contains("sunt")) ||
                      (p.title.Contains("eius") || p.body.Contains("eius")) ||
                      ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
                       (p.body.Contains("facilis") || p.body.Contains("nihil"))));

                var posts =
                    Posts
                    .Values;

                // we take the first entry plus any of the others that have the correct contents and aren't posted by admins
                // making sure that those meeting the "voluptatibus" and "voluptatem" requirements are at the end
                var postsToInclude =
                    posts
                    .Take(1)
                    .Concat(posts
                            .Skip(1)
                            .Where(predicate))
                    .GroupBy(p => ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
                                   (p.body.Contains("facilis") || p.body.Contains("nihil"))) ? 2 : 1)
                    .OrderBy(g => g.Key)
                    .SelectMany(p => p)
                    .ToList();

                _suntPosts = postsToInclude;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        private static void PopulatePostCollection()
        {
            Dictionary<int,Post> posts = new Dictionary<int, Post>();

            try
            {
                posts = _dataRetriever.GetAllPosts().ToDictionary(p => p.id);
                _logger.Information($"Filled cache");
            }
            catch(Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public Post GetPost(int id, bool refresh = false)
        {
            _logger.Information("Returning single post for requested id");

            if (refresh && _posts.Count > 0 )
                _posts.Clear();

            Post post = null;

            try
            {
                post = Posts[id];
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

            return post;
        }

        public List<Post> GetSuntPosts(bool refresh = false)
        {
            _logger.Information("Returning cached sunt report");

            if (refresh && _suntPosts.Count > 0)
                _suntPosts.Clear();

            List<Post> suntPosts = null;

            try
            {
                suntPosts = SuntPosts;
            }
            catch(Exception e)
            {
                _logger.Error(e, e.Message);
            }

            return suntPosts;
        }
    }
}
