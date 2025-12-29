using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VaultController : ControllerBase
{
    private readonly IVaultRepository _repo;

    public VaultController(IVaultRepository repo)
    {
        _repo = repo;
    }

    // ADMIN: puede ver todo
    [HttpGet("all")]
    [Authorize(Roles = "admin")]
    public IActionResult GetAll()
    {
        var items = _repo.GetAll();
        return Ok(items);
    }

    // USER: solo ve sus propios datos
    [HttpGet("mine")]
    [Authorize]
    public IActionResult GetMine()
    {
        var username = User.Identity?.Name;

        if (username == null)
            return Unauthorized("Access denied");

        var items = _repo.GetByOwner(username);
        return Ok(items);
    }
}