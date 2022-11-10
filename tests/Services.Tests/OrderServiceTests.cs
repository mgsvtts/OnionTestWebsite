using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task Create_Throws_NumberAndProviderIdIsNotUniqueException_If_Needed()
        {
            var repositoryManagerMock = new Mock<IRepositoryManager>();
            repositoryManagerMock.Setup(x=>x.ProviderRepository.NumberIsUniqueAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                                 .Returns(false);
            var service = new OrderService(repositoryManagerMock.Object);


            var function =async ()=>await service.CreateAsync(new OrderForCreationDto ());


            await Assert.ThrowsAsync<NumberAndProviderIdIsNotUniqueException>(function);
        }

        [Fact]
        public async Task Delete_Throws_OrderNotFoundException_If_Needed()
        {
            var repositoryManagerMock = new Mock<IRepositoryManager>();
            repositoryManagerMock.Setup(x=>x.OrderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                                 .Returns((Order)null);
            var service = new OrderService(repositoryManagerMock.Object);


            var function = async ()=> await service.DeleteAsync(-1);


            await Assert.ThrowsAsync<OrderNotFoundException>(function);
        }
    }
}
