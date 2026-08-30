using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlanchisserieAPI.DTOs;
using BlanchisserieAPI.Services;

namespace BlanchisserieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<ActionResult<List<OrderItemDto>?>> GetAllItems()
        {
            var items = await _orderItemService.GetAllItemsAsync();

            if (items == null)
            {
                return NotFound(new { message = "Aucun article trouvé" });
            }

            return Ok(items);
        }

        [HttpGet("get/{orderId}")]
        [Authorize]
        public async Task<ActionResult<List<OrderItemDto>?>> GetItemsByOrderId(int orderId)
        {
            var items = await _orderItemService.GetAllItemsByOrderIdAsync(orderId);

            if (items == null)
            {
                return NotFound(new { message = "Aucun article trouvé pour cette commande" });
            }

            return Ok(items);
        }

        [HttpDelete("remove/{itemId}")]
        [Authorize(Roles = "Admin")] // only admins are able to remove items from the catalogue
        public async Task<ActionResult> RemoveItemFromCatalogue(int itemId){
            var result = await _orderItemService.RemoveItemFromCatalogueAsync(itemId);

            if (result == 0)
            {
                return NotFound(new { message = "Article non trouvé dans le catalogue" });
            }

            return Ok(new { message = "Article supprimé du catalogue avec succès" });
        }
    }
}
