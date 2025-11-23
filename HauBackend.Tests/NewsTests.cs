//using Xunit;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using hau_backend.Controllers;
//using hau_backend.Data;
//using hau_backend.Models;

//namespace hau_backend.Tests
//{
//    public class NewsTests
//    {
//        private AppDbContext GetInMemoryDbContext()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
//                .Options;

//            var context = new AppDbContext(options);

//            context.News.AddRange(new List<News>
//            {
//                new News { Id = 1, Title = "Title 1", Content = "Content 1" },
//                new News { Id = 2, Title = "Title 2", Content = "Content 2" }
//            });

//            context.SaveChanges();
//            return context;
//        }

//        [Fact]
//        public async Task GetNews_ReturnsAllNews()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext();
//            var controller = new NewsController(context);

//            // Act
//            var result = await controller.GetNews();

//            // Assert
//            var newsItems = Assert.IsType<ActionResult<IEnumerable<News>>>(result);
//            Assert.NotNull(newsItems.Value);
//        }

//        [Fact]
//        public async Task GetNewsById_ReturnsNewsById_WhenFound()
//        {
//            var context = GetInMemoryDbContext();
//            var controller = new NewsController(context);

//            var result = await controller.GetNewsById(1);
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var newsItem = Assert.IsType<News>(okResult.Value);
//            Assert.Equal(1, newsItem.Id);
//        }

//        [Fact]
//        public async Task GetNewsById_ReturnsNotFound_WhenMissing()
//        {
//            var context = GetInMemoryDbContext();
//            var controller = new NewsController(context);

//            var result = await controller.GetNewsById(99999);
//            Assert.IsType<NotFoundResult>(result.Result);
//        }

//        [Fact]
//        public async Task CreateNews_ReturnsOk_WhenValid()
//        {
//            var context = GetInMemoryDbContext();
//            var controller = new NewsController(context);

//            var newNews = new News { Title = "New", Content = "New Content" };

//            var result = await controller.CreateNews(newNews);
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var createdNews = Assert.IsType<News>(okResult.Value);

//            Assert.Equal("New", createdNews.Title);
//        }

//        [Fact]
//        public async Task DeleteNews_RemovesNews_WhenFound()
//        {
//            var context = GetInMemoryDbContext();
//            var controller = new NewsController(context);

//            var result = await controller.DeleteNews(1);
//            Assert.IsType<NoContentResult>(result);

//            var deleted = await context.News.FindAsync(1);
//            Assert.Null(deleted);
//        }
//    }
//}
