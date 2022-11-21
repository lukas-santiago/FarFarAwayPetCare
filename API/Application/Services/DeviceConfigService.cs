using Application.Configuration;
using Application.Errors;
using Application.Models;
using Application.Models.View;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class DeviceConfigService: IDeviceConfigService
{
    private readonly ApiContext _connection;

    public DeviceConfigService(ApiContext connection)
    {
        _connection = connection;
    }

    public async Task<DeviceConfig> Add(DeviceConfig value)
    {
        if (value.Id != 0)
            throw new AlreadyExistsException("Configuração do Dispositivo já existente");

        await _connection.DeviceConfig.AddAsync(value);
        await _connection.SaveChangesAsync();

        return value;
    }

    public async Task Delete(int id)
    {
        var entity = await _connection.DeviceConfig.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Configuração do Dispositivo não encontrado");


        _connection.DeviceConfig.Remove(entity);
        await _connection.SaveChangesAsync();
    }

    public async Task<DeviceConfig> Edit(DeviceConfig value)
    {
        var entity = await _connection.DeviceConfig.FindAsync(value.Id);

        if (entity == null)
            throw new NotFoundException("Configuração do Dispositivo não encontrado");

        entity.Periodicidade = value.Periodicidade;
        entity.UpdatedDate = DateTime.Now;

        _connection.DeviceConfig.Update(entity);
        await _connection.SaveChangesAsync();

        return entity;
    }

    public async Task<DeviceConfig> Get(int id)
    {
        var entity = await _connection.DeviceConfig.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Configuração do Dispositivo não encontrado");

        return entity;
    }

    public async Task<IEnumerable<DeviceConfig>> GetAll()
    {
        List<DeviceConfig> result = _connection.DeviceConfig.OrderBy(f => f.Id).ToList();
        return await Task.FromResult(result);
    }

    public Task<DeviceConfig> GetByDeviceConfigType(int DeviceConfigTypeId)
    {
        var entity = _connection.DeviceConfig.Where(e => e.DeviceConfigTypeId == DeviceConfigTypeId).First();

        if (entity == null)
            throw new NotFoundException("Configuração do Dispositivo não encontrado");

        return Task.FromResult(entity);
    }

    public Task<List<DeviceConfig>> GetByDevice(int DeviceId)
    {
        var entity = _connection.DeviceConfig.Where(e => e.DeviceId == DeviceId).ToList();

        if (entity == null)
            throw new NotFoundException("Configuração do Dispositivo não encontrado");

        return Task.FromResult(entity);
    }

    public async Task<object?> Save(DeviceConfigView value)
    {
        await _connection.DeviceConfig.Where(e => e.DeviceId == value.DeviceId).ExecuteDeleteAsync();
        foreach (var deviceConfig in value.deviceConfigs)
        {
            await Add(new DeviceConfig()
            {
                DeviceId = value.DeviceId,
                Device = _connection.Device.Find(value.DeviceId),
                DeviceConfigTypeId = deviceConfig.DeviceConfigTypeId,
                DeviceConfigType = _connection.DeviceConfigType.Find(deviceConfig.DeviceConfigTypeId),
                Periodicidade = deviceConfig.Periodicidade,
                extras = deviceConfig.extras,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            });
        }
        return "";
    }
}
