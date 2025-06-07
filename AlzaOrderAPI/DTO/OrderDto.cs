using AlzaOrderAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AlzaOrderAPI.DTO
{
    public class OrderDto
    {
        [Required]
        [MaxLength(100)] //Asi obmedzené na FE pri tvorbe objednávky/zákazníckeho účtu, ale pre istotu
        public string CustomerName { get; set; }
        [Required]
        //[MaxLength(24)]//"YYYY-MM-DD HH:MM:SS.SSS")    
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
