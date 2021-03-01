using System;
using System.Linq;
using System.Threading.Tasks;
using Application.User;
using Application.User.Handlers;
using FluentAssertions;
using Infrastructure.Database;
using Infrastructure.Providers.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Application.Tests
{
    public class UserTests
    {
        private readonly UserService _sut;
        
        public UserTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new DatabaseContext(options);
            context.Database.EnsureCreated();
            var logger = new Mock<ILogger<UserService>>();
            var clock = new Mock<IClockProvider>();
            
            _sut = new UserService(context, logger.Object, clock.Object);
        }

        [Fact]
        public void GetUserById_EmptyId_ShouldReturnException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _sut.GetUserById(Guid.Empty));
        }

        [Fact]
        public async Task GetUserById_Should_Return_User()
        {
           // Arrange
            var user = new RegisterUserQuery
            {
                Email = "craig@listerhome.com",
                Firstname = "Craig",
                Password = "password",
                Surname = "Lister",
                CountryId = 0
            };
            var result = await _sut.Register(user);
            
            // Act
            var users = await _sut.GetUserById(result.UserId);
            
            // Assert
            users.Data.Email.Current.Should().Be(user.Email);
            users.Data.Email.CurrentIsValidated.Should().BeFalse();
        }

        [Fact]
        public async Task  RegisterUser_IsAssignedUserRoleOnly()
        {
            // Arrange
            var newUser = new RegisterUserQuery
            {
                Email = "craig@listerhome.com",
                Firstname = "Craig",
                Password = "password",
                Surname = "Lister",
                CountryId = 0
            };
            
            // Act
            var saveResult = await _sut.Register(newUser);
            var result = await _sut.GetUserById(saveResult.UserId);
            
            // Assert
            result.Data.Roles.Should().ContainSingle();
            result.Data.Roles.FirstOrDefault()?.Name.Should().Be("User");
        }

        [Fact]
        public async Task Registration_Email_RequiresActivation()
        {
            // Arrange
            var newUser = new RegisterUserQuery
            {
                Email = "craig@listerhome.com",
                Firstname = "Craig",
                Password = "password",
                Surname = "Lister",
                CountryId = 0
            };
            
            // Act
            var saveResult = await _sut.Register(newUser);
            var result = await _sut.GetUserById(saveResult.UserId);
            
            // Assert
            result.Data.Email.CurrentIsValidated.Should().BeFalse();
        }
    }
}