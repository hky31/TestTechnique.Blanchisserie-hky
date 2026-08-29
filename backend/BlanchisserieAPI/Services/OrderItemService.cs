using Microsoft.EntityFrameworkCore;
using BlanchisserieAPI.Data;
using BlanchisserieAPI.DTOs;
using BlanchisserieAPI.Models;

namespace BlanchisserieAPI.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItemDto>?> GetAllItemsByOrderIdAsync(int orderId);
        Task<List<OrderItemDto>?> GetAllItemsAsync();
    }

    public class OrderItemService : IOrderItemService
    {
        private readonly ApplicationDbContext _context;

        public OrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItemDto>?> GetAllItemsAsync()
        {
            var allItems = await _context.OrderItems
                .Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ItemName = item.ItemName,
                    Price = item.Price
                })
                .ToListAsync();

            if (allItems == null || !allItems.Any())
                return null;

            return allItems;
        }

        public async Task<List<OrderItemDto>?> GetAllItemsByOrderIdAsync(int orderId)
        {
            var items = await _context.OrderOrderItems
                .Include(oo => oo.OrderItem)
                .Include(o => o.Order)
                .Where(o => o.OrderId == orderId)
                .Select(oo => new OrderItemDto
                {
                    Id = oo.Id,
                    ItemName = oo.OrderItem.ItemName,
                    Price = oo.OrderItem.Price
                })
                .ToListAsync();

            if (items == null || !items.Any())
                return null;

            return items;
        }
    }
}
