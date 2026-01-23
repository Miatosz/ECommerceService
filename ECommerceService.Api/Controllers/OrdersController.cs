using ECommerceService.Api.Dto;
using ECommerceService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService srv)
        {
            _orderService = srv;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);

            return CreatedAtAction(nameof(GetOrderById), new { id = order }, order);
        }

        [HttpPut("{id}/updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderDto dto, int id)
        {
            await _orderService.ChangeOrderStatusAsync(id, dto);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetOrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderService.CancelOrder(id);
            return Ok();
        }

        [HttpPost("{id}/send")]
        public async Task<IActionResult> SendOrder(int id)
        {
            await _orderService.SendOrder(id);
            return Ok();
        }
    }
}

