namespace Auxquimia.xUnit.Security
{
    using IdentityModel;
    using IdentityServer4.Validation;
    using Moq;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Security;
    using Auxquimia.Service.Authentication;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="ResourceOwnerPasswordValidatorTests" />
    /// </summary>
    public class ResourceOwnerPasswordValidatorTests
    {
        /// <summary>
        /// The ShouldReturnInvalidGrantWhenCredentialsAreIncorrect
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ShouldReturnInvalidGrantWhenCredentialsAreIncorrect()
        {
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            ResourceOwnerPasswordValidator validator = new ResourceOwnerPasswordValidator(userServiceMock.Object);

            ResourceOwnerPasswordValidationContext context = new ResourceOwnerPasswordValidationContext
            {
                UserName = "the-user",
                Password = "the-password"
            };
            await validator.ValidateAsync(context);
            Assert.NotNull(context.Result);
            Assert.True(context.Result.IsError);
        }

        /// <summary>
        /// The ShouldReturnValidGrantWhenCredentialsAreCorrect
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ShouldReturnValidGrantWhenCredentialsAreCorrect()
        {
            string expectedSubject = "the-user";

            UserDto expectedUser = new UserDto
            {
                Username = expectedSubject,
                Enabled = true,
                AccountNonExpired = true,
                AccountNonLocked = true,
                CredentialsNonExpired = true
            };
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedUser);

            ResourceOwnerPasswordValidator validator = new ResourceOwnerPasswordValidator(userServiceMock.Object);

            ResourceOwnerPasswordValidationContext context = new ResourceOwnerPasswordValidationContext
            {
                UserName = expectedSubject,
                Password = "the-password"
            };
            await validator.ValidateAsync(context);
            Assert.NotNull(context.Result);
            Assert.False(context.Result.IsError);
            Assert.NotNull(context.Result.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject));
            Assert.Equal(expectedSubject, context.Result.Subject.Claims.First(c => c.Type == JwtClaimTypes.Subject).Value);
        }
    }
}
