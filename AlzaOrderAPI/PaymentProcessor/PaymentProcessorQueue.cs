namespace AlzaOrderAPI.PaymentProcessor
{
    sealed class PaymentProcessorQueue
    {
        private static PaymentProcessorQueue _instance;
        private static readonly object _lock = new object();
        private Queue<KeyValuePair<uint, bool>> queue; // ConcurrentQueue je zbytočná pre toto zadanie

        private PaymentProcessorQueue()
        {
            queue = new Queue<KeyValuePair<uint, bool>>();
        }

        public static PaymentProcessorQueue GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PaymentProcessorQueue();
                    }
                }
                _instance = new PaymentProcessorQueue();
            }
            return _instance;
        }

        public void Enqueue(KeyValuePair<uint,bool> orderPayment)
        {
            queue.Enqueue(orderPayment);
        }

        public KeyValuePair<uint,bool>? Dequeue()
        {
            if(queue.Count > 0) return queue.Dequeue();
            return null;
        }
    }
}
