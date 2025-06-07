using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlzaOrderAPI.Models
{
    public class OrderItem
    {
        [Key]
        public uint OrderItemId { get; set; }        

        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double PricePerPiece { get; set; }
        public uint OrderId { get; set; }        
        public virtual Order Order { get; set; }
    }
}
