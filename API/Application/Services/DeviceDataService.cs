using Application.Configuration;
using Application.Errors;
using Application.Models;
using Application.Services.Interfaces;

namespace Application.Services;

public class DeviceDataService: IDeviceDataService
{
    private readonly ApiContext _connection;

    public DeviceDataService(ApiContext connection)
    {
        _connection = connection;
    }

    public async Task<DeviceData> Add(DeviceData value)
    {
        if (value.Id != 0)
            throw new AlreadyExistsException("Dado do Dispositivo já existente");

        await _connection.DeviceData.AddAsync(value);
        await _connection.SaveChangesAsync();

        return value;
    }

    public async Task Delete(int id)
    {
        var entity = await _connection.DeviceData.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Dado do Dispositivo não encontrado");


        _connection.DeviceData.Remove(entity);
        await _connection.SaveChangesAsync();
    }

    public async Task<DeviceData> Edit(DeviceData value)
    {
        var entity = await _connection.DeviceData.FindAsync(value.Id);

        if (entity == null)
            throw new NotFoundException("Dado do Dispositivo não encontrado");

        entity.Nome = value.Nome;
        entity.Value = value.Value;
        entity.UpdatedDate = DateTime.Now;

        _connection.DeviceData.Update(entity);
        await _connection.SaveChangesAsync();

        return entity;
    }

    public async Task<DeviceData> Get(int id)
    {
        var entity = await _connection.DeviceData.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Dado do Dispositivo não encontrado");

        return entity;
    }

    public async Task<IEnumerable<DeviceData>> GetAll()
    {
        List<DeviceData> result = _connection.DeviceData.OrderBy(f => f.Id).ToList();
        return await Task.FromResult(result);
    }

    public Task<List<DeviceData>> GetByDeviceConfig(int DeviceConfigId)
    {
        var entity = _connection.DeviceData.Where(e => e.DeviceConfigId == DeviceConfigId).ToList();

        if (entity == null)
            throw new NotFoundException("Dado do Dispositivo não encontrado");

        return Task.FromResult(entity);
    }

    public Task<List<DeviceData>> GetByDevice(int DeviceId)
    {
        var deviceConfig = _connection.DeviceConfig.Where(e => e.DeviceId == DeviceId).ToList();
        var entity = _connection.DeviceData.Where(e => deviceConfig.Contains(e.DeviceConfig)).ToList();

        if (entity == null)
            throw new NotFoundException("Dado do Dispositivo não encontrado");

        return Task.FromResult(entity);
    }
}