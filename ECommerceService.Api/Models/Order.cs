namespace ECommerceService.Api.Models
{
    public class Order
    {
        public Order()
        {
            OrderDate = DateTime.Now;
            OrderStatus = Status.Ordered;
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } = null;

        public DateTime OrderDate { get; set; }
        public Status OrderStatus { get; set; }

        public decimal Total { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


        public void ChangeStatus(Status newStatus)
        {
            if (OrderStatus == Status.Sent)
                throw new InvalidOperationException();

            OrderStatus = newStatus;
        }
    }

    public enum Status { Ordered = 1, Preparing = 2, Sent = 3 }


}
