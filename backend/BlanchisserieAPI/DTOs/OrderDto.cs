using System.ComponentModel.DataAnnotations;
using BlanchisserieAPI.Models;

namespace BlanchisserieAPI.DTOs
{
    // DTO for returning order response -- reponse de la validation d'une commande
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Waiting;
        public string Commentaire { get; set; } = string.Empty;
    }

    // DTO for querying orders with details -- effectuer une commande
    public class OrderRequestDto
    {
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Waiting;
        public string Commentaire { get; set; } = string.Empty;
    }
}
