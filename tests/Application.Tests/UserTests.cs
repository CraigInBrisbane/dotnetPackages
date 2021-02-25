using System;
using System.Linq;
using System.Threading.Tasks;
using Application.User;
using Application.User.Handlers;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
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
        private readonly Guid _userId = Guid.NewGuid();
        
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
        public async Task GetUserById_Should_Return_Users()
        {
           // Setup
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
            var users = await _sut.GetUsers();
            
            // Assert
            Assert.Single(users.Data);
            Assert.Equal(user.Email, users.Data?.FirstOrDefault().Email.Current);
            Assert.Equal(false, users.Data?.FirstOrDefault().Email.CurrentIsValidated);
        }

        [Fact]
        public async Task  RegisterUser_IsAssignedUserRoleOnly()
        {
            // Setup
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
            Assert.Single(result.Data.Roles);
            Assert.Equal("User", result.Data.Roles.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task Registration_Email_RequiresActivation()
        {
            // Setup
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
            
            Assert.False(result.Data.Email.CurrentIsValidated);
        }
    }
}