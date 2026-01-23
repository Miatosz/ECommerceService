namespace ECommerceService.Api.Models
{
    public class OrderItem
    {
        protected OrderItem() { }

        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; } = null;

        public int ProductId { get; set; }
        public Product? Product { get; set; } = null;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        internal OrderItem(int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
