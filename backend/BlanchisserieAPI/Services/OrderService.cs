using Microsoft.EntityFrameworkCore;
using BlanchisserieAPI.Data;
using BlanchisserieAPI.DTOs;
using BlanchisserieAPI.Models;

namespace BlanchisserieAPI.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto?> GetOrderByIdAsync(int orderid);
        Task<List<OrderResponseDto>?> GetAllOrdersAsync();
        Task<OrderResponseDto?> CreateOrderAsync(OrderRequestDto orderRequest);
        Task<OrderResponseDto?> UpdateOrderAsync(int orderId, OrderRequestDto orderRequest);
        Task<List<OrderResponseDto>?> GetOrdersByUserIdAsync(int userId);
    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderResponseDto>?> GetAllOrdersAsync()
        {
            var allOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .Select(order => new OrderResponseDto
                {
                    Id = order.Id,
                    OrderItems = order.OrderItems.ToList(),
                    CreatedAt = order.CreatedAt,
                    Status = order.Status,
                    Commentaire = order.Commentaire
                })
                .ToListAsync();

            if(allOrders == null || !allOrders.Any())
                return null;
            
            return allOrders;

        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderid)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderid);

            if (order == null)
                return null;
            
            return new OrderResponseDto
            {
                Id = order.Id,
                OrderItems = order.OrderItems.ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Commentaire = order.Commentaire
            };
        }

        public async Task<OrderResponseDto?> CreateOrderAsync(OrderRequestDto orderRequest)
        {
            // create new order
            var newOrder = new Order
            {
                CreatedAt = DateTime.UtcNow,
                Status = orderRequest.Status,
                Commentaire = orderRequest.Commentaire
            };

            // update context with new order
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // Add OrderItems
            foreach (var item in orderRequest.OrderItems)
            {
                var newItem = new OrderItem
                {
                    ArticleName = item.ArticleName,
                    Price = item.Price,
                    OrderId = newOrder.Id
                };
                _context.OrderItems.Add(newItem);
            }

            // Save changes to the context
            await _context.SaveChangesAsync();

            return new OrderResponseDto
            {
                Id = newOrder.Id,
                OrderItems = newOrder.OrderItems.ToList(),
                CreatedAt = newOrder.CreatedAt,
                Status = newOrder.Status,
                Commentaire = newOrder.Commentaire
            };
        }

        public async Task<OrderResponseDto?> UpdateOrderAsync(int orderId, OrderRequestDto orderRequest)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;    
            
            // Update order details
            order.Status = orderRequest.Status;
            order.Commentaire = orderRequest.Commentaire;
            
            // Save changes to the context
            await _context.SaveChangesAsync();

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderItems = order.OrderItems.ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Commentaire = order.Commentaire
            };
        }

        public async Task<List<OrderResponseDto>?> GetOrdersByUserIdAsync(int userId)
        {
            var userOrderList = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .Select(order => new OrderResponseDto
                {
                    Id = order.Id,
                    OrderItems = order.OrderItems.ToList(),
                    CreatedAt = order.CreatedAt,
                    Status = order.Status,
                    Commentaire = order.Commentaire
                })
                .ToListAsync();

            if (userOrderList == null)
                return null;

            return userOrderList;
        }
    }
}
