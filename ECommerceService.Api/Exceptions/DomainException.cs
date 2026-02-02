namespace ECommerceService.Api.Exceptions
{
    public class DomainException : Exception
    {
        public string? Code { get; }

        public DomainException(string message, string? code = null) : base(message)
        {
            Code = code;
        }
    }

    public class ProductNotFoundException : DomainException
    {
        public int ProductId { get; }

        public ProductNotFoundException(int productId)
            : base($"Product with ID {productId} not found.", "PRODUCT_NOT_FOUND")
        {
            ProductId = productId;
        }
    }

    public class InsufficientStockException : DomainException
    {
        public int ProductId { get; }
        public int RequiredQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientStockException(int productId, int required, int available)
            : base(
                $"Insufficient stock for product {productId}. Required: {required}, Available: {available}.",
                "INSUFFICIENT_STOCK")
        {
            ProductId = productId;
            RequiredQuantity = required;
            AvailableQuantity = available;
        }
    }

    public class OrderNotModifiableException : DomainException
    {
        public int OrderId { get; }

        public OrderNotModifiableException(int orderId)
            : base($"Order {orderId} cannot be modified.", "ORDER_NOT_MODIFIABLE")
        {
            OrderId = orderId;
        }
    }

    public class InvalidOrderStateException : DomainException
    {
        public InvalidOrderStateException(string message)
            : base(message, "INVALID_ORDER_STATE")
        {
        }
    }
}