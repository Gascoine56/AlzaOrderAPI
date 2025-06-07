

using AlzaOrderAPI.Data;
using AlzaOrderAPI.Enums;
using AlzaOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlzaOrderAPI.PaymentProcessor
{
    public class PaymentProcessorBackgroundWorker : BackgroundService
    {
        readonly ILogger<PaymentProcessorBackgroundWorker> _logger;
        private PaymentProcessorQueue _paymentProcessorQueue;
        readonly IServiceProvider _serviceProvider;
        public PaymentProcessorBackgroundWorker(ILogger<PaymentProcessorBackgroundWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _paymentProcessorQueue = PaymentProcessorQueue.GetInstance();
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Processing order ");
                await ProcessPayment();
            }
        }

        private async Task ProcessPayment()
        {
            
            var queueItem = _paymentProcessorQueue.Dequeue();
            if (queueItem == null)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                return;
            }

            try
            {
                KeyValuePair<uint, bool> orderPaymentToProcess = (KeyValuePair<uint, bool>)queueItem;

                using var scope = _serviceProvider.CreateScope();
                var orderDbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                Order orderToProcess = await orderDbContext.Orders
                    .Where(o => o.OrderId == orderPaymentToProcess.Key).SingleAsync();
                //Pokud je zaplacení true, nastavíme stav objednávky Paid, pokud False, nastavíme Cancelled
                //Možno by bola ešte na mieste kontrola, či už náhodou nie je objednávka spracovaná               
                switch (orderPaymentToProcess.Value)
                {
                    case true:
                        orderToProcess.OrderState = (int)OrderStateEnum.Paid;
                            break;
                    case false:
                        orderToProcess.OrderState = (int)OrderStateEnum.Cancelled;
                        break;                       
                }

                await orderDbContext.SaveChangesAsync();
            }
            catch
            {
                //Vrátit do queue špatný request nechcem, logujem chybu
                _logger.LogInformation($"Failed to process Order : {queueItem.ToString()}");
            }

        }
    }
}
