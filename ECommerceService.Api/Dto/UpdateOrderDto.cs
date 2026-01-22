using ECommerceService.Api.Models;

namespace ECommerceService.Api.Dto
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public Status OrderStatus { get; set; }
    }
}
