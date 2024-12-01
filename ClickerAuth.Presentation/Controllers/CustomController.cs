using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickerAuth.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomController(IMediator mediator)
{
    [Authorize]
    [HttpGet(nameof(GetInt))]
    public async Task<int> GetInt()
    {
        return 5;
    }
}