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


            //
            // var user = new Infrastructure.Database.Entities.User
            // {
            //     Created = clock.Object.UtcNow(),
            //     Firstname = "Craig",
            //     Id = _userId,
            //     Password = "password",
            //     Surname = "Lister",
            //     CountryId = 0,
            // };
            //
            // var userRoles = new UserRole
            // {
            //     Created = clock.Object.UtcNow(),
            //     Role = context.Roles.FirstOrDefault(x => x.Id == 1),
            //     User = user
            // };
            //
            // var userEmail = new UserEmail
            // {
            //     Created = clock.Object.UtcNow(),
            //     Email = "craig@listerhome.com",
            //     User = user,
            //     ValidatedDate = clock.Object.UtcNow().AddHours(1)
            // };
            //
            // context.Users.Add(user);
            // context.UserRoles.Add(userRoles);
            // context.UserEmails.Add(userEmail);
            //
            // context.SaveChanges();
            
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
    }
}