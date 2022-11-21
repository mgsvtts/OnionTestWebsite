using Contracts.Order;
using Contracts.OrderItem;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;

namespace Services.Tests;

public class OrderServiceTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;

    public OrderServiceTests()
    {
        var repositoryManagerMock = new Mock<IRepositoryManager>();
        repositoryManagerMock.Setup(x => x.OrderRepository.GetAllContainsNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()).Result)
                             .Returns(new List<Order>());
        repositoryManagerMock.Setup(x => x.ProviderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                             .Returns(new Provider());

        _repositoryManagerMock = repositoryManagerMock;
    }

    [Fact]
    public async Task Create_Throws_NumberAndProviderIdIsNotUniqueException()
    {
        var provider = new Provider { Id = 1, Name = "provider" };
        var order = new Order { Provider = provider, ProviderId = provider.Id };
        var repositoryManagerMock = new Mock<IRepositoryManager>();
        repositoryManagerMock.Setup(x => x.OrderRepository.GetAllWithExactlySameNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()).Result)
                             .Returns(new List<Order> { order });
        repositoryManagerMock.Setup(x => x.ProviderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                             .Returns(provider);
        var service = new OrderService(repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto());

        await Assert.ThrowsAsync<NumberAndProviderIdIsNotUniqueException>(function);
    }

    [Fact]
    public async Task Create_Throws_OrderItemNumberEqualOrderNameException()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto
        {
            Number = "Same",
            Date = DateTime.Today,
            OrderItems = new List<OrderItemForCreationDto>
            {
                new OrderItemForCreationDto {
                    Name = "Same",
                    Quantity = "Not null",
                    Unit = "Not null"
                }
            }
        });
        
        await Assert.ThrowsAsync<OrderItemNumberEqualOrderNameException>(function);
    }

    [Fact]
    public async Task Create_Throws_NumberIsNull()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto { Number = null });

        await Assert.ThrowsAsync<ArgumentException>(function);
    }

    [Fact]
    public async Task Create_Throws_DateIsInvalid()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var minValueFunction = async () => await service.CreateAsync(new OrderForCreationDto { Number = "Not null", Date = DateTime.MinValue });
        var maxValueFunction = async () => await service.CreateAsync(new OrderForCreationDto { Number = "Not null", Date = DateTime.MaxValue });

        await Assert.ThrowsAsync<FormatException>(minValueFunction);
        await Assert.ThrowsAsync<FormatException>(maxValueFunction);
    }

    [Fact]
    public async Task Create_Throws_ParametersCannotBeNull()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto
        {
            Date = DateTime.Today,
            Number = "Not null",
            OrderItems = new List<OrderItemForCreationDto> { new OrderItemForCreationDto() }
        });

        await Assert.ThrowsAsync<ArgumentException>(function);
    }

    [Fact]
    public async Task Create_Throws_UseComma()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto
        {
            Number = "Not null",
            OrderItems = new List<OrderItemForCreationDto>
            {
                new OrderItemForCreationDto
                {
                    Quantity = "1,111",
                    Unit = "Not",
                    Name = "null"
                }
            }
        });

        await Assert.ThrowsAsync<FormatException>(function);
    }

    [Fact]
    public async Task Create_Throws_QuantityMustHaveLessThan3Decimals()
    {
        var service = new OrderService(_repositoryManagerMock.Object);

        var function = async () => await service.CreateAsync(new OrderForCreationDto
        {
            Number = "Not null",
            Date = DateTime.Today,
            OrderItems = new List<OrderItemForCreationDto>
            {
                new OrderItemForCreationDto
                {
                    Quantity = "0,1234",
                    Name = "Not",
                    Unit = "null"
                }
            }
        });

        await Assert.ThrowsAsync<FormatException>(function);
    }

    [Fact]
    public async Task Delete_Throws_OrderNotFoundException()
    {
        var repositoryManagerMock = new Mock<IRepositoryManager>();
        repositoryManagerMock.Setup(x => x.OrderRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).Result)
                             .Returns((Order)null);
        var service = new OrderService(repositoryManagerMock.Object);

        var function = async () => await service.DeleteAsync(-1);

        await Assert.ThrowsAsync<OrderNotFoundException>(function);
    }
}