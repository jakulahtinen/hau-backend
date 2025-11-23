//using Xunit;
//using Microsoft.AspNetCore.Mvc;
//using hau_backend.Controllers;
//using hau_backend.Models;
//using Microsoft.Extensions.Options;
//using Microsoft.Extensions.Configuration;

//namespace hau_backend.Tests
//{
//    public class AuthTests
//    {
//        private AuthController GetControllerWithCredentials(string validUsername, string validPassword)
//        {
//            var inMemorySettings = new Dictionary<string, string?>
//                {
//                    {"AdminCredentials:Username", validUsername},
//                    {"AdminCredentials:Password", validPassword}
//                };

//            IConfiguration configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(inMemorySettings)
//                .Build();

//            var jwtSettings = new JwtSettings
//            {
//                Secret = "supersecurejwtsecretkeywithminimum32chars",
//                Issuer = "TestIssuer",
//                Audience = "TestAudience"
//            };

//            var options = Options.Create(jwtSettings);
//            return new AuthController(configuration, options);
//        }

//        [Fact]
//        public void Login_WithValidCredentials_ReturnsToken()
//        {
//            var controller = GetControllerWithCredentials("admin", "password");

//            var request = new LoginRequest
//            {
//                Username = "admin",
//                Password = "password"
//            };

//            var result = controller.Login(request);

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.NotNull(okResult.Value);
//        }

//        [Fact]
//        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
//        {
//            var controller = GetControllerWithCredentials("admin", "password");

//            var request = new LoginRequest
//            {
//                Username = "wrongUser",
//                Password = "wrongPassword"
//            };

//            var result = controller.Login(request);

//            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
//            Assert.Equal("Käyttäjänimi tai salasana on virheellinen", unauthorizedResult.Value);
//        }

//        [Fact]
//        public void Login_WithNullRequest_ReturnsUnauthorized()
//        {
//            var controller = GetControllerWithCredentials("admin", "password");

//            var result = controller.Login(new LoginRequest());

//            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
//            Assert.Equal("Käyttäjänimi tai salasana on virheellinen", unauthorizedResult.Value);
//        }
//    }
//}