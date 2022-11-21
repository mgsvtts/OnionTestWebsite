using Contracts.OrderItem;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Services;

public class OrderItemService : IOrderItemService
{
    private readonly IRepositoryManager _repositoryManager;

    public OrderItemService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task CreateAsync(OrderItemForCreationDto itemDto, CancellationToken token = default)
    {
        await ValidateOrderItemDto(itemDto, token);

        _repositoryManager.OrderItemRepository.Add(itemDto.Adapt<OrderItem>());

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(OrderItemForCreationDto itemDto, CancellationToken token = default)
    {
        await ValidateOrderItemDto(itemDto, token);

        var item =await _repositoryManager.OrderItemRepository.GetByIdAsync(itemDto.Id, token);

        item.Name = itemDto.Name;
        item.Quantity = decimal.Parse(itemDto.Quantity);
        item.Unit= itemDto.Unit;

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        var item = await _repositoryManager.OrderItemRepository.GetByIdAsync(id, token);

        _repositoryManager.OrderItemRepository.Remove(item);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    public async Task<OrderItemDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        var item = await _repositoryManager.OrderItemRepository.GetByIdAsync(id, token);

        return item.Adapt<OrderItemDto>();
    }

    public async Task<IEnumerable<OrderItemDto>> GetAllAsync(CancellationToken token = default)
    {
        var items = await _repositoryManager.OrderItemRepository.GetAllAsync(token);

        return items.Adapt<IEnumerable<OrderItemDto>>();
    }

    public async Task<IEnumerable<OrderItemDto>> GetAllByOrderIdAsync(int orderId, CancellationToken token = default)
    {
        var items = await _repositoryManager.OrderItemRepository.GetAllByOrderIdAsync(orderId, token);

        return items.Adapt<IEnumerable<OrderItemDto>>();
    }

    public async Task<IEnumerable<string>> GetAllUnitsAsync(CancellationToken token = default)
    {
        var items = await _repositoryManager.OrderItemRepository.GetAllAsync(token);

        return (from item in items select item.Unit).ToList();
    }

    public async Task<IEnumerable<string>> GetAllNamesAsync(CancellationToken token = default)
    {
        var items = await _repositoryManager.OrderItemRepository.GetAllAsync(token);

        return (from item in items select item.Name).ToList();
    }

    private async Task ValidateOrderItemDto(OrderItemForCreationDto orderItemDto, CancellationToken token = default)
    {
        var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderItemDto.OrderId, token);

        if (order == null)
        {
            throw new OrderNotFoundException(orderItemDto.OrderId);
        }
        
        if (string.IsNullOrEmpty(orderItemDto.Name) || string.IsNullOrEmpty(orderItemDto.Unit) || string.IsNullOrEmpty(orderItemDto.Quantity))
        {
            throw new ArgumentException("Order item parameters cannot be null");
        }

        if (order.Number == orderItemDto.Name)
        {
            throw new OrderItemNumberEqualOrderNameException();
        }

        if (!string.IsNullOrEmpty(orderItemDto.Quantity) && orderItemDto.Quantity.Contains('.'))
        {
            throw new FormatException("Use comma instead of dot in quantity");
        }

        if (!Regex.IsMatch(orderItemDto.Quantity, @"^[0-9]+(\,[0-9]{1,3})?$"))
        {
            throw new FormatException("Valid number with maximum 3 decimal places");
        }
    }
}