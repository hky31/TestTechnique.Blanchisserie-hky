using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlanchisserieAPI.Attributes;

namespace BlanchisserieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("admin-only")]
        [RequireRole("Admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new { message = "Accès autorisé pour les administrateurs uniquement" });
        }

        [HttpGet("user-or-admin")]
        [RequireRole("Admin", "Utilisateur")]
        public IActionResult UserOrAdmin()
        {
            return Ok(new { message = "Accès autorisé pour les utilisateurs et administrateurs" });
        }

        [HttpGet("authenticated")]
        public IActionResult Authenticated()
        {
            return Ok(new { message = "Accès autorisé pour tous les utilisateurs authentifiés" });
        }
    }
}
