using AlzaOrderAPI.Data;
using AlzaOrderAPI.DTO;
using AlzaOrderAPI.Models;
using AlzaOrderAPI.PaymentProcessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlzaOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _orderDbContext;

       public OrderController(OrderDbContext orderDbContext)
        {
            this._orderDbContext = orderDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            List<Order> ordersList = await _orderDbContext.Orders.Include(o => o.OrderItems).ToListAsync();
            return ordersList;
        }
        
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order can not be null");
            }
            var newOrder = new Order(orderDto);

            try
            {
                var x = _orderDbContext.Orders.Add(newOrder);
                await _orderDbContext.SaveChangesAsync();
                return Ok(orderDto);//Záleží, či na FE chceme zobraziť údaje z objednávky, možno zbytočne posielať
            }
            catch (Exception e)
            {
               return Problem(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrderPaymentState(uint orderId, bool orderPaidFor)
        {
            if (await _orderDbContext.Orders.AnyAsync(o => o.OrderId == orderId) == false)
            {
                return BadRequest("Order ID does not exist");
            }
            PaymentProcessorQueue paymentProcessorQueue = PaymentProcessorQueue.GetInstance();
            paymentProcessorQueue.Enqueue(new KeyValuePair<uint, bool>(orderId, orderPaidFor));
            return Ok();
        }
    }
}
