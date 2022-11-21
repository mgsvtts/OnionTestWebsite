using Contracts.Order;
using Contracts.OrderItem;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;

namespace Services.Tests;

public class OrderItemServiceTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;

    public OrderItemServiceTests()
    {
        var repositoryManagerMock = new Mock<IRepositoryManager>();
        repositoryManagerMock.Setup(x => x.OrderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                             .Returns(new Order());

        _repositoryManagerMock = repositoryManagerMock;
    }

    [Fact]
    public async Task Create_Throws_OrderNotFound()
    {
        var repositoryManagerMock = new Mock<IRepositoryManager>();
        repositoryManagerMock.Setup(x => x.OrderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                             .Returns((Order)null);
        var service = new OrderItemService(repositoryManagerMock.Object);
        var function = async () => await service.CreateAsync(new OrderItemForCreationDto());

        await Assert.ThrowsAsync<OrderNotFoundException>(function);
    }

    [Fact]
    public async Task Create_Throws_OrderItemNumberEqualOrderNameException()
    {
        _repositoryManagerMock.Setup(x=>x.OrderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                              .Returns(new Order { Number = "Same"});
        var service = new OrderItemService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderItemForCreationDto 
        { 
            Name = "Same",
            Quantity = "Not null",
            Unit = "Not null"
            
        });
            
        await Assert.ThrowsAsync<OrderItemNumberEqualOrderNameException>(function);
    }

    [Fact]
    public async Task Create_Throws_ParametersCannotBeNull()
    {
        var service = new OrderItemService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderItemForCreationDto());

        await Assert.ThrowsAsync<ArgumentException>(function);
    }

    [Fact]
    public async Task Create_Throws_QuantityMustHaveLessThan3Decimals()
    {
        var service = new OrderItemService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderItemForCreationDto
        {
            Quantity = "0,1234",
            Name = "Not",
            Unit = "null"
        });

        await Assert.ThrowsAsync<FormatException>(function);
    }

    [Fact]
    public async Task Create_Throws_UseComma()
    {
        var service = new OrderItemService(_repositoryManagerMock.Object);
        var function = async () => await service.CreateAsync(new OrderItemForCreationDto
        {
            Quantity = "1.111",
            Name = "Not",
            Unit = "null"
        });

        await Assert.ThrowsAsync<FormatException>(function);
    }
}