using Application.Attributes;
using Application.Controllers.Interfaces;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceDataController : ControllerBase, ICrudController<DeviceData>

{
    public IDeviceDataService _service { get; }
    public DeviceDataController(IDeviceDataService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());

    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _service.Get(id));
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DeviceData value)
    {
        return Ok(await _service.Add(value));
    }
    [HttpPost("FromDevice")]
    [AllowAnonymous]
    [DeviceApiKey]
    public async Task<IActionResult> AddFromDevice([FromBody] DeviceData value)
    {
        return Ok(await _service.Add(value));
    }
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] DeviceData value)
    {
        return Ok(await _service.Edit(value));
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}
