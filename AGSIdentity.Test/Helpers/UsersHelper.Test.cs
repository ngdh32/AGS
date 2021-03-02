using System;
using Xunit;
using AGSIdentity.Helpers;
using Moq;
using AGSIdentity.Repositories;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using System.Linq;
using Microsoft.Extensions.Configuration;
using AGSIdentity.Models.ViewModels.API.Common;

namespace AGSIdentity.Test.Helpers
{
    public class UsersHelperTest
    {
        public static IConfiguration _configuration
        {
            get
            {
                var myConfiguration = new Dictionary<string, string>
                {
                    {"default_user_password", "123456"}
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(myConfiguration)
                    .Build();

                return configuration;
            }
        }


        public UsersHelperTest()
        {
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetUserById_ValidId_Success(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }

            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            var result = usersHelper.GetUserById(id);
            Assert.NotNull(result);

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetUserById_EmptyOrNullId_ThrowException(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }

            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.GetUserById(id));

        }


        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void GetUserById_InvalidId_ReturnNull(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }

            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            var result = usersHelper.GetUserById(id);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void ResetPassword_ValidId_Success(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.ResetPassword(user.Id, _configuration["default_user_password"])).Returns(true);
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }

            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            var result = usersHelper.ResetPassword(id);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ResetPassword_EmptyOrNullId_ThrowException(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }


            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.ResetPassword(id));
        }

        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void ResetPassword_InvalidId_ThrowException(string id)
        {
            // mock the IRepository object start
            var usersRepository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                usersRepository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }


            var usersHelper = new UsersHelper(usersRepository.Object, _configuration);
            Assert.Throws<AGSException>(() => usersHelper.ResetPassword(id));
        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetGroupsByUserId_ValidId_Success(string id)
        {
            var repository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }
            
            foreach(var group in MockData.groups)
            {
                repository.Setup(_ => _.GroupsRepository.Get(group.Id)).Returns(group);
            }

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var result = usersHelper.GetGroupsByUserId(id);

            Assert.NotNull(result);

        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetGroupsByUserId_EmptyOrNullId_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();
            foreach (var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }

            foreach (var group in MockData.groups)
            {
                repository.Setup(_ => _.GroupsRepository.Get(group.Id)).Returns(group);
            }

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.GetGroupsByUserId(id));
        }


        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void GetGroupsByUserId_InvalidId_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.Get(
                It.Is<string>(y => !(MockData.users.Any(x => x.Id == y))))
                ).Returns((AGSUserEntity)null);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<AGSException>(() => usersHelper.GetGroupsByUserId(id));
        }

        [Fact]
        public void GetAllUsers_HaveUsers_Success()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.GetAll()).Returns(MockData.users.Select(x => x.Id).ToList());

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var result = usersHelper.GetAllUsers();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllUsers_NoUsers_ReturnEmptyList()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.GetAll()).Returns((List<string>)null);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var result = usersHelper.GetAllUsers();
            Assert.Empty(result);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ChangePassword_EmptyOrNullUserId_ThrowException(string userId)
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.ChangePassword(userId, "abc", "abc"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ChangePassword_EmptyOrNullOldPassword_ThrowException(string oldPassword)
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.ChangePassword("abc", oldPassword, "abc"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ChangePassword_EmptyOrNullNewPassword__ThrowException(string newPassword)
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.ChangePassword("abc", "abc", newPassword));

        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void ChangePassword_ValidInput_Success(string id)
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.ValidatePassword(id, _configuration["default_user_password"])).Returns(true);
            repository.Setup(_ => _.UsersRepository.ChangePassword(id, _configuration["default_user_password"])).Returns(true);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var result = usersHelper.ChangePassword(id, _configuration["default_user_password"], _configuration["default_user_password"]);
            Assert.True(result);
        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void ChangePassword_WrongOldPassword_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.ValidatePassword(id, _configuration["default_user_password"])).Returns(false);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<AGSException>(() => usersHelper.ChangePassword(id, _configuration["default_user_password"], _configuration["default_user_password"]));
        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void UpdateUser_ValidId_Success(string id)
        {
            var repository = new Mock<IRepository>();
            foreach(var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.GetByUsername(user.Username)).Returns(user);
                repository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
                repository.Setup(_ => _.UsersRepository.Update(user)).Returns(1);
            }

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser = MockData.users.FirstOrDefault(x => x.Id == id);
            var result = usersHelper.UpdateUser(updateUser);

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateUser_EmptyOrNUllId_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser = new AGSUserEntity()
            {
                Id = id
            };
            Assert.Throws<ArgumentException>(() => usersHelper.UpdateUser(updateUser));

        }


        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void UpdateUser_InvalidId_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.Get(It.IsAny<string>())).Returns((AGSUserEntity)null);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser = new AGSUserEntity()
            {
                Id = id
            };
            Assert.Throws<AGSException>(() => usersHelper.UpdateUser(updateUser));
        }


        [Fact]
        public void UpdateUser_ChangeAdminUsername_ThrowException()
        {
            var adminUser = MockData.users.FirstOrDefault(x => x.Username == CommonConstant.AGSAdminName);

            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.Get(adminUser.Id)).Returns(adminUser);
            repository.Setup(_ => _.UsersRepository.GetByUsername(adminUser.Username)).Returns(adminUser);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser = new AGSUserEntity()
            {
                Id = adminUser.Id,
                Username = adminUser.Username + "1"
            };

            Assert.Throws<ArgumentException>(() => usersHelper.UpdateUser(updateUser));
        }


        [Fact]
        public void UpdateUser_DuplicateUsername_ThrowException()
        {
            var updateUser = MockData.users.FirstOrDefault(x => x.Username != CommonConstant.AGSAdminName);

            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.Get(updateUser.Id)).Returns(updateUser);
            repository.Setup(_ => _.UsersRepository.GetByUsername(updateUser.Username)).Returns(updateUser);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser2 = new AGSUserEntity()
            {
                Id = MockData.users.FirstOrDefault(x => x.Id != updateUser.Id && x.Username != CommonConstant.AGSAdminName).Id,
                Username = updateUser.Username
            };

            Assert.Throws<AGSException>(() => usersHelper.UpdateUser(updateUser2));
        }

        [Theory]
        [InlineData("2")]
        [InlineData("3")]
        public void UpdateUser_ChangeUsernameToAdmin_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();
            foreach(var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
                repository.Setup(_ => _.UsersRepository.GetByUsername(user.Username)).Returns(user);
            }

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var updateUser = new AGSUserEntity()
            {
                Id = id,
                Username = CommonConstant.AGSAdminName
            };

            Assert.Throws<ArgumentException>(() => usersHelper.UpdateUser(updateUser));
        }


        [Fact]
        public void CreateUser_IdNotEmpty_ThrowException()
        {
            var repository = new Mock<IRepository>();
            var newUser = new AGSUserEntity()
            {
                Id = CommonConstant.GenerateId()
            };

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentException>(() => usersHelper.CreateUser(newUser));

        }


        [Fact]
        public void CreateUser_EmptyId_Success()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.Create(It.IsAny<AGSUserEntity>())).Returns(CommonConstant.GenerateId());

            var newUser = new AGSUserEntity()
            {
                Username = "userabcde"
            };

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            var result = usersHelper.CreateUser(newUser);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateUser_DuplicateUsername_ThrowException()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(_ => _.UsersRepository.GetByUsername(It.IsAny<string>())).Returns(new AGSUserEntity());

            var newUser = new AGSUserEntity()
            {
                Username = "userabcde"
            };

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<AGSException>(() => usersHelper.CreateUser(newUser));
        }

        [Fact]
        public void CreateUser_UsernameAdmin_ThrowException()
        {
            var repository = new Mock<IRepository>();

            var newUser = new AGSUserEntity()
            {
                Username = CommonConstant.AGSAdminName
            };
            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentException>(() => usersHelper.CreateUser(newUser));
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateUser_EmptyOrNullUsername_ThrowException(string username)
        {
            var repository = new Mock<IRepository>();
            foreach(var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.GetByUsername(user.Username)).Returns(user);
            }

            var newUser = new AGSUserEntity()
            {
                Username = username
            };

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentException>(() => usersHelper.CreateUser(newUser));
        }

        [Fact]
        public void CreateUser_NullModel_ThrowException()
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.CreateUser(null));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void DeleteUser_ValidId_Success(string id)
        {
            var repository = new Mock<IRepository>();
            foreach(var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.Delete(user.Id));
            }

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            usersHelper.DeleteUser(id);
            Assert.True(true);

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DeleteUser_EmptyOrNullId_ThrowException(string id)
        {
            var repository = new Mock<IRepository>();

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentNullException>(() => usersHelper.DeleteUser(id));
        }

        [Fact]
        public void DeleteUser_DeleteAdmin_ThrowException()
        {
            var repository = new Mock<IRepository>();
            foreach(var user in MockData.users)
            {
                repository.Setup(_ => _.UsersRepository.Get(user.Id)).Returns(user);
            }
            
            var adminUser = MockData.users.FirstOrDefault(x => x.Username == CommonConstant.AGSAdminName);

            var usersHelper = new UsersHelper(repository.Object, _configuration);
            Assert.Throws<ArgumentException>(() => usersHelper.DeleteUser(adminUser.Id));
        }

    }
}
