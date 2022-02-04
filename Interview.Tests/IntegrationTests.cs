using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Interview.Controllers;
using Interview.Models;
using Serilog;
using Moq;
using Newtonsoft.Json;

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
            var mockLogger = new Mock<Serilog.ILogger>();

            string mockPostValue = @"{
    ""userId"": 5,
    ""id"": 50,
    ""title"": ""repellendus qui recusandae incidunt voluptates tenetur qui omnis exercitationem"",
    ""body"": ""error suscipit maxime adipisci consequuntur recusandae\nvoluptas eligendi et est et voluptates\nquia distinctio ab amet quaerat molestiae et vitae\nadipisci impedit sequi nesciunt quis consectetur""
  }";
            Post mockPost = JsonConvert.DeserializeObject<Post>(mockPostValue);

            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(m => m.GetPost(It.IsAny<int>())).Returns(mockPost);
            var sut = new PostsController(mockLogger.Object, mockRepository.Object);
            int id = 50;

            // act
            var result = sut.Get(id);

            // assert
            Assert.Equal(id, result.id);
            Assert.Contains("repellendus qui recusandae", result.title);
        }

        private List<Post> DeriveSuntReportPostList()
        {
            var adminIds = new int[] { 1, 8, 9 };

            //var posts =
            //    PostRepository
            //    .Posts
            //    .Values;

            //Func<Post, bool> predicate = p =>
            //     !adminIds.Contains(p.userId) &&
            //     ((p.title.Contains("sunt") || p.body.Contains("sunt")) ||
            //      (p.title.Contains("eius") || p.body.Contains("eius")) ||
            //      ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
            //       (p.body.Contains("facilis") || p.body.Contains("nihil"))));

            //// we take the first entry plus any of the others that have the correct contents and aren't posted by admins
            //// making sure that those meeting the "voluptatibus" and "voluptatem" requirements are at the end
            //var postsToInclude =
            //    posts
            //    .Take(1)
            //    .Concat(posts
            //            .Skip(1)
            //            .Where(predicate))
            //    .GroupBy(p => ((p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
            //                   (p.body.Contains("facilis") || p.body.Contains("nihil"))) ? 2 : 1)
            //    .OrderBy(g => g.Key)
            //    .SelectMany(p => p)
            //    .ToList();

            //string serializedJson = JsonConvert.SerializeObject(postsToInclude);
            string serializedJson = @"
[{""userId"":1,
""id"":1,
""title"":""sunt aut facere repellat provident occaecati excepturi optio reprehenderit"",
""body"":""quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto""
},
{""userId"":2,
""id"":13,
""title"":""dolorum ut in voluptas mollitia et saepe quo animi"",
""body"":""aut dicta possimus sint mollitia voluptas commodi quo doloremque\niste corrupti reiciendis voluptatem eius rerum\nsit cumque quod eligendi laborum minima\nperferendis recusandae assumenda consectetur porro architecto ipsum ipsam""
},
{""userId"":3,
""id"":24,
""title"":""autem hic labore sunt dolores incidunt"",
""body"":""enim et ex nulla\nomnis voluptas quia qui\nvoluptatem consequatur numquam aliquam sunt\ntotam recusandae id dignissimos aut sed asperiores deserunt""
},
{""userId"":3,
""id"":29,
""title"":""iusto eius quod necessitatibus culpa ea"",
""body"":""odit magnam ut saepe sed non qui\ntempora atque nihil\naccusamus illum doloribus illo dolor\neligendi repudiandae odit magni similique sed cum maiores""
},
{""userId"":3,
""id"":30,
""title"":""a quo magni similique perferendis"",
""body"":""alias dolor cumque\nimpedit blanditiis non eveniet odio maxime\nblanditiis amet eius quis tempora quia autem rem\na provident perspiciatis quia""
},
{""userId"":4,
""id"":31,
""title"":""ullam ut quidem id aut vel consequuntur"",
""body"":""debitis eius sed quibusdam non quis consectetur vitae\nimpedit ut qui consequatur sed aut in\nquidem sit nostrum et maiores adipisci atque\nquaerat voluptatem adipisci repudiandae""
},
{""userId"":4,
""id"":32,
""title"":""doloremque illum aliquid sunt"",
""body"":""deserunt eos nobis asperiores et hic\nest debitis repellat molestiae optio\nnihil ratione ut eos beatae quibusdam distinctio maiores\nearum voluptates et aut adipisci ea maiores voluptas maxime""
},
{""userId"":5,
""id"":41,
""title"":""non est facere"",
""body"":""molestias id nostrum\nexcepturi molestiae dolore omnis repellendus quaerat saepe\nconsectetur iste quaerat tenetur asperiores accusamus ex ut\nnam quidem est ducimus sunt debitis saepe""
},
{""userId"":5,
""id"":42,
""title"":""commodi ullam sint et excepturi error explicabo praesentium voluptas"",
""body"":""odio fugit voluptatum ducimus earum autem est incidunt voluptatem\nodit reiciendis aliquam sunt sequi nulla dolorem\nnon facere repellendus voluptates quia\nratione harum vitae ut""
},
{""userId"":5,
""id"":45,
""title"":""ut numquam possimus omnis eius suscipit laudantium iure"",
""body"":""est natus reiciendis nihil possimus aut provident\nex et dolor\nrepellat pariatur est\nnobis rerum repellendus dolorem autem""
},
{""userId"":5,
""id"":49,
""title"":""laborum non sunt aut ut assumenda perspiciatis voluptas"",
""body"":""inventore ab sint\nnatus fugit id nulla sequi architecto nihil quaerat\neos tenetur in in eum veritatis non\nquibusdam officiis aspernatur cumque aut commodi aut""
},
{""userId"":6,
""id"":51,
""title"":""soluta aliquam aperiam consequatur illo quis voluptas"",
""body"":""sunt dolores aut doloribus\ndolore doloribus voluptates tempora et\ndoloremque et quo\ncum asperiores sit consectetur dolorem""
},
{""userId"":6,
""id"":53,
""title"":""ut quo aut ducimus alias"",
""body"":""minima harum praesentium eum rerum illo dolore\nquasi exercitationem rerum nam\nporro quis neque quo\nconsequatur minus dolor quidem veritatis sunt non explicabo similique""
},
{""userId"":6,
""id"":58,
""title"":""voluptatum itaque dolores nisi et quasi"",
""body"":""veniam voluptatum quae adipisci id\net id quia eos ad et dolorem\naliquam quo nisi sunt eos impedit error\nad similique veniam""
},
{""userId"":6,
""id"":59,
""title"":""qui commodi dolor at maiores et quis id accusantium"",
""body"":""perspiciatis et quam ea autem temporibus non voluptatibus qui\nbeatae a earum officia nesciunt dolores suscipit voluptas et\nanimi doloribus cum rerum quas et magni\net hic ut ut commodi expedita sunt""
},
{""userId"":6,
""id"":60,
""title"":""consequatur placeat omnis quisquam quia reprehenderit fugit veritatis facere"",
""body"":""asperiores sunt ab assumenda cumque modi velit\nqui esse omnis\nvoluptate et fuga perferendis voluptas\nillo ratione amet aut et omnis""
},
{""userId"":7,
""id"":65,
""title"":""consequatur id enim sunt et et"",
""body"":""voluptatibus ex esse\nsint explicabo est aliquid cumque adipisci fuga repellat labore\nmolestiae corrupti ex saepe at asperiores et perferendis\nnatus id esse incidunt pariatur""
},
{""userId"":7,
""id"":70,
""title"":""voluptatem laborum magni"",
""body"":""sunt repellendus quae\nest asperiores aut deleniti esse accusamus repellendus quia aut\nquia dolorem unde\neum tempora esse dolore""
},
{""userId"":10,
""id"":91,
""title"":""aut amet sed"",
""body"":""libero voluptate eveniet aperiam sed\nsunt placeat suscipit molestias\nsimilique fugit nam natus\nexpedita consequatur consequatur dolores quia eos et placeat""
},
{""userId"":10,
""id"":93,
""title"":""beatae soluta recusandae"",
""body"":""dolorem quibusdam ducimus consequuntur dicta aut quo laboriosam\nvoluptatem quis enim recusandae ut sed sunt\nnostrum est odit totam\nsit error sed sunt eveniet provident qui nulla""
},
{""userId"":10,
""id"":98,
""title"":""laboriosam dolor voluptates"",
""body"":""doloremque ex facilis sit sint culpa\nsoluta assumenda eligendi non ut eius\nsequi ducimus vel quasi\nveritatis est dolores""
},
{""userId"":2,
""id"":17,
""title"":""fugit voluptas sed molestias voluptatem provident"",
""body"":""eos voluptas et aut odit natus earum\naspernatur fuga molestiae ullam\ndeserunt ratione qui eos\nqui nihil ratione nemo velit ut aut id quo""
},
{""userId"":3,
""id"":21,
""title"":""asperiores ea ipsam voluptatibus modi minima quia sint"",
""body"":""repellat aliquid praesentium dolorem quo\nsed totam minus non itaque\nnihil labore molestiae sunt dolor eveniet hic recusandae veniam\ntempora et tenetur expedita sunt""
},
{""userId"":5,
""id"":48,
""title"":""ut voluptatem illum ea doloribus itaque eos"",
""body"":""voluptates quo voluptatem facilis iure occaecati\nvel assumenda rerum officia et\nillum perspiciatis ab deleniti\nlaudantium repellat ad ut et autem reprehenderit""
},
{""userId"":6,
""id"":55,
""title"":""sit vel voluptatem et non libero"",
""body"":""debitis excepturi ea perferendis harum libero optio\neos accusamus cum fuga ut sapiente repudiandae\net ut incidunt omnis molestiae\nnihil ut eum odit""
},
{""userId"":7,
""id"":61,
""title"":""voluptatem doloribus consectetur est ut ducimus"",
""body"":""ab nemo optio odio\ndelectus tenetur corporis similique nobis repellendus rerum omnis facilis\nvero blanditiis debitis in nesciunt doloribus dicta dolores\nmagnam minus velit""
}]";

            var postsToInclude = JsonConvert.DeserializeObject<List<Post>>(serializedJson);
            return postsToInclude;
        }

        [Fact]
        // temporary test just to support initial refactorings
        // temporarily uses application to fetch data, so not good long-term test
        // ultimately we will inject mock data
        public void ReportHasNoAdminEntriesExceptFirstPost()
        { 
            // arrange
            var mockLogger = new Mock<Serilog.ILogger>();
            PostRepository.Configure(mockLogger.Object);

            List<Post> postsToInclude = DeriveSuntReportPostList();
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(m => m.GetSuntPosts(It.IsAny<bool>())).Returns(postsToInclude);
            var sut = new PostsController(mockLogger.Object, mockRepository.Object );

            // act
            var result = sut.AllWithSunt();

            // assert
            Assert.Equal(postsToInclude.Count(), result.Data.Count());
            Assert.True(result.Data.All(p => postsToInclude.Select(pi => pi.id).Contains(p.id)));
        }

       [Fact] 
        public void AllVoluptatibusAndVoluptatemPostsComeAtTheEnd()
        {
            // arrange
            var mockLogger = new Mock<Serilog.ILogger>();

            List<Post> postsToInclude = DeriveSuntReportPostList();
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(m => m.GetSuntPosts(It.IsAny<bool>())).Returns(postsToInclude);
            var sut = new PostsController(mockLogger.Object, mockRepository.Object);

            // act
            var result = sut.AllWithSunt();

            // assert
            Assert.Equal(postsToInclude.Count(), result.Data.Count());

            Func<Post,bool> predicate =
                (p => (p.title.Contains("voluptatibus") || p.title.Contains("voluptatem")) &&
                      (p.body.Contains("facilis") || p.body.Contains("nihil")));

            // ensure all "voluptatibus"/"voluptatem" entries are at the end
            Assert.Empty(result.Data
                        .Reverse()
                        .SkipWhile(predicate )
                        .TakeWhile(predicate));
        }
    }
}
