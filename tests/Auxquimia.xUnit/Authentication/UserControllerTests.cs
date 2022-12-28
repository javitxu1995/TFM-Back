namespace Auxquimia.xUnit
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using Auxquimia.Controllers.Authentication;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Service.Authentication;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="UserControllerTests" />
    /// </summary>
    public class UserControllerTests
    {
        /// <summary>
        /// The GetByIdShouldReturnBadResponseWhenUserIsNull
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task GetByIdShouldReturnBadResponseWhenUserIsNull()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(actionResult);
        }

        /// <summary>
        /// The GetByIdShouldReturnOkWhenUserIsNotNull
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task GetByIdShouldReturnOkWhenUserIsNotNull()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            userService.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new UserDto
            {
                Id = Guid.NewGuid().ToString()
            });
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.GetById(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(actionResult);
            OkObjectResult okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult.Value);
        }

        /// <summary>
        /// The SaveShouldNotAcceptPayloadsWithNonNullId
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task SaveShouldNotAcceptPayloadsWithNonNullId()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.Save(new UserDto
            {
                Id = "non-null-id"
            });
            Assert.IsType<BadRequestResult>(actionResult);
        }

        /// <summary>
        /// The UpdateShouldNotAcceptPayloadsWithNullId
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task UpdateShouldNotAcceptPayloadsWithNullId()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.Update(new UserDto());
            Assert.IsType<BadRequestResult>(actionResult);
        }

        /// <summary>
        /// The UpdateShouldReturnOkObjectResult
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task UpdateShouldReturnOkObjectResult()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            userService.Setup(x => x.UpdateAsync(It.IsAny<UserDto>())).ReturnsAsync(new UserDto());
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.Update(new UserDto
            {
                Id = "non-null-id"
            });
            Assert.IsType<OkObjectResult>(actionResult);
        }

        /// <summary>
        /// The SaveShouldReturnOkObjectResult
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task SaveShouldReturnOkObjectResult()
        {
            Mock<IUserService> userService = new Mock<IUserService>();
            userService.Setup(x => x.SaveAsync(It.IsAny<UserDto>())).ReturnsAsync(new UserDto());
            UserController controller = new UserController(userService.Object, new NullLogger<UserController>());

            IActionResult actionResult = await controller.Save(new UserDto());
            Assert.IsType<OkObjectResult>(actionResult);
        }
    }
}
