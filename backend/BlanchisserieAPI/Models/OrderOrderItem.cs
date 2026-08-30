using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlanchisserieAPI.Models
{
    // this class represents the link between an order and its items
    public class OrderOrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        
        public DateTime OrderedAt { get; set; }
    }
}
