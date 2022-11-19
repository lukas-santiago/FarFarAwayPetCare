using Application.Controllers.Interfaces;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceConfigController : ControllerBase, ICrudController<DeviceConfig>
{
    public IDeviceConfigService _service { get; }
    public DeviceConfigController(IDeviceConfigService service)
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
    public async Task<IActionResult> Add([FromBody] DeviceConfig value)
    {
        return Ok(await _service.Add(value));
    }
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] DeviceConfig value)
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
