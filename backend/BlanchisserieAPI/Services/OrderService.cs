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
        Task<OrderResponseDto?> CreateOrderAsync(OrderRequestDto orderRequest, int userId);
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
                .Include(o => o.OrderList)
                .Include(o => o.User)
                .Select(order => new OrderResponseDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    CustomerFirstName = order.User != null ? order.User.FirstName : string.Empty,
                    CustomerLastName = order.User != null ? order.User.LastName : string.Empty,
                    CustomerEmail = order.User != null ? order.User.Email : string.Empty,
                    OrderItems = order.OrderList.Select(oo => oo.OrderItem).ToList(),
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
                .Include(o => o.OrderList)
                .ThenInclude(oo => oo.OrderItem)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderid);

            if (order == null)
                return null;
            
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CustomerFirstName = order.User != null ? order.User.FirstName : string.Empty,
                CustomerLastName = order.User != null ? order.User.LastName : string.Empty,
                CustomerEmail = order.User != null ? order.User.Email : string.Empty,
                OrderItems = order.OrderList.Select(oo => oo.OrderItem).ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Commentaire = order.Commentaire
            };
        }

        public async Task<OrderResponseDto?> CreateOrderAsync(OrderRequestDto orderRequest, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            // create new order
            var newOrder = new Order
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                Status = OrderStatus.Waiting,
                Commentaire = orderRequest.Commentaire
            };

            // update context with new order
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // Add OrderOrderItems
            foreach (var orderItemId in orderRequest.OrderItemIds)
            {
                var orderItem = await _context.OrderItems.FindAsync(orderItemId);
                if (orderItem != null)
                {
                    var orderOrderItem = new OrderOrderItem
                    {
                        OrderId = newOrder.Id,
                        OrderItemId = orderItem.Id
                    };
                    _context.OrderOrderItems.Add(orderOrderItem);
                }
            }

            // Save changes to the context
            await _context.SaveChangesAsync();

            // Rechargement explicite, cohérent avec les autres méthodes
            var createdOrder = await _context.Orders
                .Include(o => o.OrderList)
                .ThenInclude(oo => oo.OrderItem)
                .Include(o => o.User)
                .FirstAsync(o => o.Id == newOrder.Id);

            return new OrderResponseDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                CustomerFirstName = createdOrder.User!.FirstName,
                CustomerLastName = createdOrder.User!.LastName,
                CustomerEmail = createdOrder.User!.Email,
                OrderItems = createdOrder.OrderList.Select(oo => oo.OrderItem).ToList(),
                CreatedAt = createdOrder.CreatedAt,
                Status = createdOrder.Status,
                Commentaire = createdOrder.Commentaire
            };
        }

        public async Task<OrderResponseDto?> UpdateOrderAsync(int orderId, OrderRequestDto orderRequest)
        {
            var order = await _context.Orders
                .Include(o => o.OrderList)
                .ThenInclude(oo => oo.OrderItem)
                .Include(o => o.User)
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
                UserId = order.UserId,
                CustomerFirstName = order.User != null ? order.User.FirstName : string.Empty,
                CustomerLastName = order.User != null ? order.User.LastName : string.Empty,
                CustomerEmail = order.User != null ? order.User.Email : string.Empty,
                OrderItems = order.OrderList.Select(oo => oo.OrderItem).ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Commentaire = order.Commentaire
            };
        }

        public async Task<List<OrderResponseDto>?> GetOrdersByUserIdAsync(int userId)
        {
            var userOrderList = await _context.Orders
                .Include(o => o.OrderList)
                .ThenInclude(oo => oo.OrderItem)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .Select(order => new OrderResponseDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    CustomerFirstName = order.User != null ? order.User.FirstName : string.Empty,
                    CustomerLastName = order.User != null ? order.User.LastName : string.Empty,
                    CustomerEmail = order.User != null ? order.User.Email : string.Empty,
                    OrderItems = order.OrderList.Select(oo => oo.OrderItem).ToList(),
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
