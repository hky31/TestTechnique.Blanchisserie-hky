using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlanchisserieAPI.Models
{
    // Enum to represent the status of an order
    public enum OrderStatus
    {
        Waiting = 0,
        Validated = 1,
        Refused = 2
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public ICollection<OrderOrderItem> OrderList { get; set; } = new List<OrderOrderItem>();
        
        public DateTime CreatedAt { get; set; }

        // Status of the order, default value is Waiting
        public OrderStatus Status { get; set; } = OrderStatus.Waiting;

        public string Commentaire { get; set; } = string.Empty;
        
        // An order is always linked to a user
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
