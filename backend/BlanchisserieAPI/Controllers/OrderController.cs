using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlanchisserieAPI.DTOs;
using BlanchisserieAPI.Services;

namespace BlanchisserieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin")] // Seuls les administrateurs peuvent voir toutes les commandes
        public async Task<ActionResult<List<OrderResponseDto>?>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            if (orders == null)
            {
                return NotFound(new { message = "Aucune commande trouvée" });
            }

            return Ok(orders);
        }

        [HttpGet("get/{orderId}")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto?>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
                return NotFound(new { message = $"Commande avec l'ID {orderId} non trouvée" });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && (userIdClaim == null || order.UserId != int.Parse(userIdClaim)))
                return Forbid();

            return Ok(order);
        }
        
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto?>> CreateOrder([FromBody] OrderRequestDto orderRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(orderRequest, userId);  // ⬅️ userId vient du token, pas de orderRequest

            if (order == null)
                return BadRequest(new { message = "Erreur lors de la création de la commande" });

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }

        [HttpPut("update/{orderId}")]
        [Authorize(Roles = "Admin")] // Seuls les administrateurs peuvent mettre à jour les commandes
        public async Task<ActionResult<OrderResponseDto?>> UpdateOrder(int orderId, [FromBody] OrderRequestDto orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderService.UpdateOrderAsync(orderId, orderRequest);

            if (order == null)
            {
                return BadRequest(new { message = "Erreur lors de la mise à jour de la commande" });
            }

            return Ok(order);
        }

        [HttpGet("get/user/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<OrderResponseDto>?>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orders == null)
            {
                return NotFound(new { message = $"Aucune commande trouvée pour l'utilisateur {userId}" });
            }

            return Ok(orders);
        }
    }
}
