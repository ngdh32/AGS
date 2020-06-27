using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSIdentity.Controllers.V1;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using Moq;
using Xunit;

namespace AGSIdentity.Test
{
    public class FunctionClaimBLLTest
    {
        List<AGSFunctionClaimEntity> functionClaimEntities { get; set; }

        public FunctionClaimBLLTest()
        {
            functionClaimEntities = new List<AGSFunctionClaimEntity>();
            functionClaimEntities.Add(new AGSFunctionClaimEntity()
            {
                Id = "1",
                Name = "FC 1"
            });
            functionClaimEntities.Add(new AGSFunctionClaimEntity()
            {
                Id = "2",
                Name = "FC 2"
            });
        }

        [Fact]
        public void GetModel_NullId_ThrowArugmentNullException()
        {
            var repository = new Mock<IRepository>();
            string functionClaimId = null;

            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimController.GetModel(functionClaimId));
        }

        [Fact]
        public void GetModel_ValidId_ReturnNonEmptyObject()
        {
            var repository = new Mock<IRepository>();
            string functionClaimId = "1";

            repository.Setup(x => x.FunctionClaimRepository.Get(functionClaimId)).Returns(functionClaimEntities.Find(x => x.Id == functionClaimId));
            var functionClaimController = new FunctionClaimsController(repository.Object);
            var result = functionClaimController.GetModel(functionClaimId);
            AGSFunctionClaimEntity expected = new AGSFunctionClaimEntity()
            {
                Id = functionClaimId,
                Name = "FC 1"
            };
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public void GetModel_NotFoundId_ReturnNull()
        {
            var repository = new Mock<IRepository>();
            string functionClaimId = "3";

            AGSFunctionClaimEntity data = null;
            repository.Setup(x => x.FunctionClaimRepository.Get(functionClaimId)).Returns(data);
            var functionClaimController = new FunctionClaimsController(repository.Object);
            AGSFunctionClaimEntity result = functionClaimController.GetModel(functionClaimId);

            Assert.Null(result);
        }

        [Fact]
        public void SaveModel_NullInput_ThrowArgumentNullException()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = null;
            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimController.SaveModel(input));
        }

        [Fact]
        public void SaveModel_NotNullInput_ThrowArgumentException()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "1",
                Name = "FC 1"
            };
            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentException>(() => functionClaimController.SaveModel(input));
        }

        [Fact]
        public void SaveModel_EmptyId_ReturnNewId()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "",
                Name = "FC 2"
            };

            repository.Setup(x => x.FunctionClaimRepository.Create(It.Is<AGSFunctionClaimEntity>(x => string.IsNullOrEmpty(x.Id) && x.Name == input.Name))).Returns("3");
            repository.Setup(x => x.FunctionClaimRepository.Create(It.Is<AGSFunctionClaimEntity>(x => !string.IsNullOrEmpty(x.Id) || x.Name != input.Name))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);
            var result = functionClaimController.SaveModel(input);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void SaveModel_NullId_ReturnNewId()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = null,
                Name = "FC 2"
            };

            repository.Setup(x => x.FunctionClaimRepository.Create(It.Is<AGSFunctionClaimEntity>(x => string.IsNullOrEmpty(x.Id) && x.Name == input.Name))).Returns("3");
            repository.Setup(x => x.FunctionClaimRepository.Create(It.Is<AGSFunctionClaimEntity>(x => !string.IsNullOrEmpty(x.Id) || x.Name != input.Name))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);
            var result = functionClaimController.SaveModel(input);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void UpdateModel_NullInput_ThrowArgumentNullException()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = null;
            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentNullException>(() => functionClaimController.UpdateModel(input));
        }

        [Fact]
        public void UpdateModel_NullId_ThrowArgumentException()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                 Id= null,
                 Name = "FC"
            };
            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentException>(() => functionClaimController.UpdateModel(input));
        }

        [Fact]
        public void UpdateModel_EmptyId_ThrowArgumentException()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "",
                Name = "FC"
            };
            var functionClaimController = new FunctionClaimsController(repository.Object);
            Assert.Throws<ArgumentException>(() => functionClaimController.UpdateModel(input));
        }

        [Fact]
        public void UpdateModel_ValidId_ReturnOriginalId()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "1",
                Name = "FC 1"
            };

            
            repository.Setup(x => x.FunctionClaimRepository.Update(It.Is<AGSFunctionClaimEntity>(x => x.Id == input.Id && x.Name == input.Name )));
            repository.Setup(x => x.FunctionClaimRepository.Update(It.Is<AGSFunctionClaimEntity>(x => x.Id != input.Id || x.Name != input.Name))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);
            
            try
            {
                functionClaimController.UpdateModel(input);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void UpdateModel_NotFoundId_ThrowModelNotFoundError()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "5",
                Name = "FC 1"
            };


            repository.Setup(x => x.FunctionClaimRepository.Update(It.Is<AGSFunctionClaimEntity>(x => x.Id == input.Id && x.Name == input.Name))).Returns(1);
            repository.Setup(x => x.FunctionClaimRepository.Update(It.Is<AGSFunctionClaimEntity>(x => x.Id != input.Id || x.Name != input.Name))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);
            try
            {
                functionClaimController.UpdateModel(input);
                Assert.True(true);
            }catch(Exception ex)
            {
                Assert.True(false);
            }

            
        }

        [Fact]
        public void DeleteModel_NotFoundId_NoErrorThrow()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "2",
                Name = "FC 1"
            };


            repository.Setup(x => x.FunctionClaimRepository.Delete(It.Is<string>(x => x == input.Id)));
            repository.Setup(x => x.FunctionClaimRepository.Delete(It.Is<string>(x => x != input.Id))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);

            try
            {
                functionClaimController.DeleteModel(input.Id);
                Assert.True(true);
            }
            catch(Exception ex)
            {
                Assert.True(false);
            }
            
        }

        [Fact]
        public void DeleteModel_ValidId_NoErrorThrow()
        {
            var repository = new Mock<IRepository>();

            AGSFunctionClaimEntity input = new AGSFunctionClaimEntity()
            {
                Id = "1",
                Name = "FC 1"
            };


            repository.Setup(x => x.FunctionClaimRepository.Delete(It.Is<string>(x => x == input.Id)));
            repository.Setup(x => x.FunctionClaimRepository.Delete(It.Is<string>(x => x != input.Id))).Throws(new Exception());
            var functionClaimController = new FunctionClaimsController(repository.Object);

            try
            {
                functionClaimController.DeleteModel(input.Id);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void DeleteModel_NullId_ThrowArgumentNullException()
        {
            var repository = new Mock<IRepository>();
            var functionClaimController = new FunctionClaimsController(repository.Object);

            try
            {
                Assert.Throws<ArgumentNullException>(() => functionClaimController.DeleteModel(null));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }
    }
}
