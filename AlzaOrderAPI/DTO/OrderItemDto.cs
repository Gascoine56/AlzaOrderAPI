namespace AlzaOrderAPI.DTO
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double PricePerPiece { get; set; }
    }
}
