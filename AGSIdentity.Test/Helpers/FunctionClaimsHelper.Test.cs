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
    }
}