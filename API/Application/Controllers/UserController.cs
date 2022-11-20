using Application.Models.View;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public UserService _service { get; }
    public AuthController(UserService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserView model)
    {
        var user = await _service.Get(model.Username, model.Password);
        var token = TokenService.GenerateToken(user);
        user.Password = "";
        return await Task.FromResult(new
        {
            user = user.Username,
            token = token
        });
    }
}
