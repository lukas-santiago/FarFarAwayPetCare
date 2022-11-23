using Application.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatorioColetaController : ControllerBase
{
    public ApiContext _connection { get; }
    public RelatorioColetaController(ApiContext context)
    {
        _connection = context;
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result =
            from dd in _connection.DeviceData
            group dd by dd.DeviceConfigId into gdd
            select new
            {
                DeviceConfigId = gdd.Key,
                Id = gdd.Max(x => x.Id)
            } into vdd
            join dc in _connection.DeviceConfig on vdd.DeviceConfigId equals dc.Id
            where dc.DeviceId == id
            join dct in _connection.DeviceConfigType on dc.DeviceConfigTypeId equals dct.Id
            join dd2 in _connection.DeviceData on vdd.Id equals dd2.Id
            select new
            {
                vdd.DeviceConfigId,
                vdd.Id,
                dc.DeviceId,
                dct.Nome,
                dd2.Value,
                ValueString = dd2.Nome
            } into vdd
            select vdd;

        return Ok(result.ToList());

    }
}
