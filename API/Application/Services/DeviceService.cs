using Application.Configuration;
using Application.Errors;
using Application.Models;
using Application.Services.Interfaces;

namespace Application.Services;

public class DeviceService : IDeviceService   
{
    private readonly ApiContext _connection;

    public DeviceService(ApiContext connection)
    {
        _connection = connection;
    }

    public async Task<Device> Add(Device value)
    {
        if (value.Id != 0)
            throw new AlreadyExistsException("Dispositivo já existente");

        await _connection.Device.AddAsync(value);
        await _connection.SaveChangesAsync();

        return value;
    }

    public async Task Delete(int id)
    {
        var entity = await _connection.Device.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Dispositivo não encontrado");


        _connection.Device.Remove(entity);
        await _connection.SaveChangesAsync();
    }

    public async Task<Device> Edit(Device value)
    {
        var entity = await _connection.Device.FindAsync(value.Id);

        if (entity == null)
            throw new NotFoundException("Dispositivo não encontrado");

        entity.Nome = value.Nome;
        entity.UpdatedDate = DateTime.Now;

        _connection.Device.Update(entity);
        await _connection.SaveChangesAsync();

        return entity;
    }

    public async Task<Device> Get(int id)
    {
        var entity = await _connection.Device.FindAsync(id);

        if (entity == null)
            throw new NotFoundException("Dispositivo não encontrado");

        return entity;
    }

    public async Task<IEnumerable<Device>> GetAll()
    {
        List<Device> result = _connection.Device.OrderBy(f => f.Id).ToList();
        return await Task.FromResult(result);
    }
}
