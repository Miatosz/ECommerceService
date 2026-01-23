using ECommerceService.Api.Models;

namespace ECommerceService.Api.Dto
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Status OrderStatus { get; set; }
        public decimal Total { get; set; }
        public List<GetOrderItemDto> Items { get; set; } = new();
        public int UserId { get; set; }
    }
}
