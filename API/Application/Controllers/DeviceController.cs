using Application.Attributes;
using Application.Controllers.Interfaces;
using Application.Models;
using Application.Models.View;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceController : ControllerBase, ICrudController<DeviceView>

{
    public IDeviceService _service { get; }
    IMapper _mapper;

    public DeviceController(IDeviceService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(_mapper.Map<IEnumerable<Device>, List<DeviceView>>(result));
    }
    [HttpPost("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        Device result = await _service.Get(id);
        return Ok(_mapper.Map<Device, DeviceView>(result));
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DeviceView value)
    {
        //var user = User.Identity.Name;
        return Ok(await _service.Add(_mapper.Map<DeviceView, Device>(value)));
    }
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] DeviceView value)
    {
        return Ok(await _service.Edit(_mapper.Map<DeviceView, Device>(value)));
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _service.Delete(id);
        return Ok();
    }
    [HttpGet("configuracao/{UniqueDeviceId}")]
    [DeviceApiKey]
    [AllowAnonymous]
    public async Task<object> GetConfiguration([FromRoute] string UniqueDeviceId, [FromQuery] string device_api_key)
    {
        object fullDeviceConfiguration = await _service.GetFullConfiguration(UniqueDeviceId);
        return fullDeviceConfiguration;
    }
}
