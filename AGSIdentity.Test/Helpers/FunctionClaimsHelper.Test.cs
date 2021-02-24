using System;
using Xunit;
using AGSIdentity.Helpers;
using Moq;
using AGSIdentity.Repositories;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using System.Linq;

namespace AGSIdentity.Test.Helpers
{
    public class FunctionClaimsHelperTest
    {
        private static List<AGSFunctionClaimEntity> functionClaims = new List<AGSFunctionClaimEntity>()
            {
                new AGSFunctionClaimEntity()
                {
                    Id = "1"
                },
                new AGSFunctionClaimEntity()
                {
                    Id = "2"
                },
                new AGSFunctionClaimEntity()
                {
                    Id = "3"
                }
            };

        public FunctionClaimsHelperTest()
        {
            

        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetFunctionClaimById_ValidID_Success(string id)
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            foreach(var functionClaim in functionClaims)
            {
                functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Get(functionClaim.Id)).Returns(functionClaim);
            }
            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            // end

            var result = functionClaimsHelper.GetFunctionClaimById(id);
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void GetFunctionClaimById_InvalidId_ReturnNull(string id)
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            foreach (var functionClaim in functionClaims)
            {
                functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Get(functionClaim.Id)).Returns(functionClaim);
            }
            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            // end

            var result = functionClaimsHelper.GetFunctionClaimById(id);
            Assert.Null(result);
        }

        [Fact]
        public void GetFunctionClaimById_EmptyId_ExceptionReturn()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            foreach (var functionClaim in functionClaims)
            {
                functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Get(functionClaim.Id)).Returns(functionClaim);
            }
            // end

            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimsHelper.GetFunctionClaimById(""));

        }

        [Fact]
        public void GetAllFunctionClaims_Success()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            foreach (var functionClaim in functionClaims)
            {
                functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Get(functionClaim.Id)).Returns(functionClaim);
            }
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.GetAll()).Returns(functionClaims.Select(x => x.Id).ToList());
            // end

            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var result = functionClaimsHelper.GetAllFunctionClaims();
            Assert.NotNull(result);
            Assert.Equal(functionClaims.Count(), result.Count());
        }

        [Fact]
        public void GetAllFunctionClaims_ReturnEmptyArray()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.GetAll()).Returns((List<string>)null);
            // end

            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var result = functionClaimsHelper.GetAllFunctionClaims();
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Theory]
        [InlineData("testing")]
        [InlineData("")]
        [InlineData(null)]
        public void CreateFunctionClaim_Valid_Success(string name)
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Create(It.Is<AGSFunctionClaimEntity>(g => string.IsNullOrEmpty(g.Id)))).Returns(CommonConstant.GenerateId());
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var newFunctionClaim = new AGSFunctionClaimEntity()
            {
                Name = name
            };
            var result = functionClaimsHelper.CreateFunctionClaim(newFunctionClaim);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateFunctionClaim_Null_ThrowException()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Create(It.Is<AGSFunctionClaimEntity>(g => string.IsNullOrEmpty(g.Id)))).Returns(CommonConstant.GenerateId());
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimsHelper.CreateFunctionClaim(null));
        }

        [Fact]
        public void CreateFunctionClaim_IdProvided_ThrowException()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Create(It.Is<AGSFunctionClaimEntity>(g => string.IsNullOrEmpty(g.Id)))).Returns(CommonConstant.GenerateId());
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var newFunctionClaim = new AGSFunctionClaimEntity()
            {
                Id = CommonConstant.GenerateId()
            };
            Assert.Throws<ArgumentException>(() => functionClaimsHelper.CreateFunctionClaim(newFunctionClaim));
        }

        [Theory]
        [InlineData("1", "testing")]
        [InlineData("2", "")]
        [InlineData("3", null)]
        public void UpdateFunctionClaim_Valid_Success(string id, string name)
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Update(It.Is<AGSFunctionClaimEntity>(g => functionClaims.Any(y => y.Id == g.Id)))).Returns(1);
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var updateFunctionClaim = new AGSFunctionClaimEntity()
            {
                Id = id,
                Name = name
            };
            var result = functionClaimsHelper.UpdateFunctionClaim(updateFunctionClaim);
            Assert.Equal(1, result);
        }

        [Fact]
        public void UpdateFunctionClaim_Null_ThrowException()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Update(It.Is<AGSFunctionClaimEntity>(g => functionClaims.Any(y => y.Id == g.Id)))).Returns(1);
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimsHelper.UpdateFunctionClaim(null));
        }

        [Fact]
        public void UpdateFunctionClaim_EmptyOrNullId_ThrowException()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Update(It.Is<AGSFunctionClaimEntity>(g => functionClaims.Any(y => y.Id == g.Id)))).Returns(1);
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            var updateFunctionClaim = new AGSFunctionClaimEntity()
            {
                Id = null
            };
            Assert.Throws<ArgumentException>(() => functionClaimsHelper.UpdateFunctionClaim(updateFunctionClaim));

            var updateFunctionClaim2 = new AGSFunctionClaimEntity()
            {
                Id = string.Empty
            };
            Assert.Throws<ArgumentException>(() => functionClaimsHelper.UpdateFunctionClaim(updateFunctionClaim2));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void DeleteFunctionClaim_ValidId_Success(string id)
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Delete(It.Is<string>(g => functionClaims.Any(y => y.Id == g))));
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            functionClaimsHelper.DeleteFunctionClaim(id);
            Assert.True(true);
        }


        [Fact]
        public void DeleteFunctionClaim_EmptyOrNull_ThrowException()
        {
            // mock the IRepository object start
            var functionClaimsRepository = new Mock<IRepository>();
            functionClaimsRepository.Setup(_ => _.FunctionClaimsRepository.Delete(It.Is<string>(g => functionClaims.Any(y => y.Id == g))));
            // end


            var functionClaimsHelper = new FunctionClaimsHelper(functionClaimsRepository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimsHelper.DeleteFunctionClaim(""));
            Assert.Throws<ArgumentNullException>(() => functionClaimsHelper.DeleteFunctionClaim(null));
        }
    }
}