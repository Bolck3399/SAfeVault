using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet("secure-area")]
    [Authorize]
    public IActionResult SecureArea()
    {
        if (User.IsInRole("admin"))
            return Ok("Access approved");

        return Unauthorized("Access denied");
    }
}