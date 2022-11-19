using Application.Models;
using Application.Models.Request;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public UserServices _service { get; }
    public AuthController(UserServices service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserRequest model)
    {
        var user = _service.Get(model.Username, model.Password);
        var token = TokenService.GenerateToken(user);
        user.Password = "";
        return Task.FromResult(new
        {
            user = user.Username,
            token = token
        });
    }
}
