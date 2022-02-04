using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Interview.Controllers;
using Interview.Models;

namespace Interview.Tests
{
    public class IntegrationTests
    {
        public static IEnumerable<object[]> TestIds 
        { 
            get 
            {
                var idArrays = new List<object[]>();
                var rnd = new Random();
                idArrays.Add(Enumerable.Range(1, 100)
                        .Select(i => (object)i)
                        .OrderBy(item => rnd.Next())
                        .ToArray()); ;
                return idArrays;
            } 
        }

        //[Theory]
        //[MemberData(nameof(TestIds))]
        [Fact]
        public void CanFetchIndividualPostSuccessfully()
        {
            // arrange
            var sut = new PostsController();
            int id = 50;

            // act
            var result = sut.Get(id).Result;

            // assert
            Assert.Equal(id, result.id);
            Assert.Contains("repellendus qui recusandae", result.title);
        }

        [Fact]
        // temporary test just to support initial refactorings
        // temporarily uses application to fetch data, so not good long-term test
        // ultimately we will inject mock data
        public void ReportHasNoAdminEntriesExceptFirstPost()
        {
            // arrange
            var sut = new PostsController();

            var adminIds = new int[] { 1, 8, 9 };

            var posts =
                PostRepository
                .Posts
                .Values;

            Func<Post, bool> predicate = p =>
                 !adminIds.Contains(p.userId) &&
                 ((p.title.Contains("sunt") || p.body.Contains("sunt")) ||
                  (p.title.Contains("eius") || p.body.Contains("eius")) ||
                  ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
                   (p.body.Contains("facilis") || p.body.Contains("nihil"))));

            // we take the first entry plus any of the others that have the correct contents and aren't posted by admins
            // making sure that those meeting the "voluptatibus" and "voluptatem" requirements are at the end
            var groupedPostsToInclude =
                posts
                .Take(1)
                .Concat(posts
                        .Skip(1)
                        .Where(predicate))
                .GroupBy(p => ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
                               (p.body.Contains("facilis") || p.body.Contains("nihil"))) ? 2 : 1);

            var postsToInclude =
                groupedPostsToInclude.SelectMany(p => p).ToList();


            // act
            var result = sut.AllWithSunt();

            var postToIncludeIds = postsToInclude.Select(p => p.id);
            var resultIds = result.Select(p => p.id);

            var incorrectEntries = postsToInclude.Where(p => !(result.Select(rp => rp.id).Contains(p.id)));

            // assert
            Assert.Equal(postsToInclude.Count(), result.Count());
            Assert.True(result.All(p => postsToInclude.Select(pi => pi.id).Contains(p.id)));
        }
    }
}
