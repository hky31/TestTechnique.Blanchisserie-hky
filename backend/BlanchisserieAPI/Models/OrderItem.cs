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
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; } = 0;
    }
}
