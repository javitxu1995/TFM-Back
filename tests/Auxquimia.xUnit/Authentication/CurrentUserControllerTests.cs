namespace Auxquimia.xUnit.Authentication
{
    using IdentityModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Auxquimia.Service.Authentication;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="CurrentUserControllerTests" />
    /// </summary>
    public class CurrentUserControllerTests
    {
        /// <summary>
        /// The ShouldReturnNotFoundWhenUserIsNull
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ShouldReturnNotFoundWhenUserIsNull()
        {
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            CurrentUserController controller = new CurrentUserController(userServiceMock.Object)
            {
                ControllerContext = GetAuthenticatedControllerContext()
            };

            IActionResult actionResult = await controller.CurrentUserData();
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }

        /// <summary>
        /// The ShouldReturnTheUserWhenFound
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ShouldReturnTheUserWhenFound()
        {
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new Dto.Authentication.UserDto
            {
                Id = "any-string"
            });

            CurrentUserController controller = new CurrentUserController(userServiceMock.Object)
            {
                ControllerContext = GetAuthenticatedControllerContext()
            };

            IActionResult actionResult = await controller.CurrentUserData();
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
        }

        /// <summary>
        /// The GetAuthenticatedControllerContext
        /// </summary>
        /// <returns>The <see cref="ControllerContext"/></returns>
        private ControllerContext GetAuthenticatedControllerContext()
        {
            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(x => x.Claims).Returns(new List<Claim> { new Claim(JwtClaimTypes.Subject, "the-subject") });

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userMock.Object
                }
            };
        }
    }
}
