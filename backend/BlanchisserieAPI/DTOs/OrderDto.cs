using System.ComponentModel.DataAnnotations;
using BlanchisserieAPI.Models;

namespace BlanchisserieAPI.DTOs
{
    // DTO for returning order response
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string Commentaire { get; set; } = string.Empty;
    }

    // DTO for querying orders with details -- create new order
    public class OrderRequestDto
    {
        public List<int> OrderItemIds { get; set; } = new List<int>();
        public string Commentaire { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Waiting;
    }

    // DTO for returning order item response
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
