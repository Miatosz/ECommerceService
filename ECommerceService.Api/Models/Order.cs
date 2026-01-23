using ECommerceService.Api.Dto;
using System.Linq;

namespace ECommerceService.Api.Models
{
    public class Order
    {
        protected Order() { }
        public static Order Create()
        {
            return new Order
            {
                OrderDate = DateTime.UtcNow,
                OrderStatus = Status.Ordered
            };
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } = null;

        public DateTime OrderDate { get; set; }
        public Status OrderStatus { get; set; }

        public decimal Total { get; set; }

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


        public void ChangeStatus(Status newStatus)
        {
            if (OrderStatus == Status.Sent)
                throw new InvalidOperationException();

            OrderStatus = newStatus;
        }

        public static Order Create(int userId)
        {
            return new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = Status.Ordered
            };
        }

        public void AddItem(int productId, int quantity, decimal unitPrice)
        {
            var item = new OrderItem(productId, quantity, unitPrice);
            _orderItems.Add(item);

            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            Total = _orderItems.Sum(i => i.TotalPrice);
        }
    }

    public enum Status { Ordered = 1, Preparing = 2, Paid = 3, Sent = 4, Cancelled = 5 }


}
