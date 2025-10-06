using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns the currently authenticated user's ID from the JWT token.
        /// </summary>
        protected Guid GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("Invalid or missing user ID claim in token.");
        }
    }
}
