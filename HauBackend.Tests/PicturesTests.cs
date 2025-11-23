//using Xunit;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using hau_backend.Controllers;
//using hau_backend.Data;
//using hau_backend.Models;

//namespace hau_backend.Tests
//{
//    public class PicturesTests
//    {
//        private AppDbContext GetInMemoryDbContext()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "PicturesDb_" + System.Guid.NewGuid())
//                .Options;
//            var context = new AppDbContext(options);
//            return context;
//        }

//        [Fact]
//        public async Task GetPictures_ReturnsAllPictures()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext();
//            context.Pictures.AddRange(new List<Picture>
//            {
//                new Picture { Title = "Picture 1", ImageData = [1]  },
//                new Picture { Title = "Picture 2", ImageData = [2]  }
//            });
//            await context.SaveChangesAsync();

//            var controller = new PicturesController(context);

//            // Act
//            var result = await controller.GetPictures();

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<IEnumerable<Picture>>>(result);
//            var returnValue = Assert.IsType<List<Picture>>(actionResult.Value);
//            Assert.Equal(2, returnValue.Count);
//        }

//        [Fact]
//        public async Task GetPicturesById_ReturnsCorrectPicture()
//        {
//            var context = GetInMemoryDbContext();
//            var pic = new Picture { Title = "MyPicture", ImageData = [1] };
//            context.Pictures.Add(pic);
//            await context.SaveChangesAsync();

//            var controller = new PicturesController(context);
//            var result = await controller.GetPictureById(pic.Id);

//            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnedPicture = Assert.IsType<Picture>(actionResult.Value);
//            Assert.Equal("MyPicture", returnedPicture.Title);
//        }

//        [Fact]
//        public async Task AddPicture_WithValidData_ReturnsCreated()
//        {
//            var context = GetInMemoryDbContext();
//            var controller = new PicturesController(context);

//            var newPicture = new Picture
//            {
//                Title = "New Picture",
//                ImageDataBase64 = Convert.ToBase64String(new byte[] { 1, 2, 3 })
//            };
//            var result = await controller.AddPicture(newPicture);   

//            var created = Assert.IsType<CreatedAtActionResult>(result);
//            var savedPicture = Assert.IsType<Picture>(created.Value);
//            Assert.Equal("New Picture", savedPicture.Title);
//            Assert.NotNull(savedPicture.ImageData);
//        }

//        [Fact]
//        public async Task DeletePicture_ExistingId_ReturnsNoContent()
//        {
//            var context = GetInMemoryDbContext();
//            var pic = new Picture { Title = "PictureToDelete", ImageData = [1] };
//            context.Pictures.Add(pic);
//            await context.SaveChangesAsync();

//            var controller = new PicturesController(context);
//            var result = await controller.DeletePicture(pic.Id);

//            Assert.IsType<NoContentResult>(result);
//        }
//    }
//}