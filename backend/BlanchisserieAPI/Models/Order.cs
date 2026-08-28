using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlanchisserieAPI.Models
{
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
        
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public DateTime CreatedAt { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Waiting;

        public string Commentaire { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
