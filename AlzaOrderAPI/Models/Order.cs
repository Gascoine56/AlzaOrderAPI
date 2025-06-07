using AlzaOrderAPI.DTO;
using AlzaOrderAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AlzaOrderAPI.Models
{
    public class Order
    {


        [Key]        
        public uint OrderId { get; set; } //Pre tento príklad by stačí int, pre prípadné velké množstvo záznamov by som použil napríklad ulong - unsigned, záporné nepotrebujeme, prípadne Guid
        [Required]
        [MaxLength(100)] //Asi obmedzené na FE pri tvorbe objednávky/zákazníckeho účtu, ale pre istotu
        public string CustomerName { get; set; }
        [Required]      
        public DateTime OrderDate { get; set; }
        [Required]        
        public int OrderState { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];

        public Order()
        {

        }

        public Order(OrderDto orderDto)
        {
            CustomerName = orderDto.CustomerName;
            OrderDate = DateTime.Now;
            OrderState = (int)OrderStateEnum.New;

            foreach(OrderItemDto oi in orderDto.OrderItems)
            {
                OrderItems.Add(new OrderItem()
                {
                    ProductName = oi.ProductName,
                    Amount = oi.Amount,
                    PricePerPiece = oi.PricePerPiece
                });

            }
        }
    }
}
