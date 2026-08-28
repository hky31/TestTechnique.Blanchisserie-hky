using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlanchisserieAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; } = 0;
        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
