using Application.Models;
using Application.Models.View;

namespace Application.Services.Interfaces;

public interface ICrudService<T> where T : class
{
    public Task<IEnumerable<T>> GetAll();
    public Task<T> Get(int id);
    public Task<T> Add(T value);
    public Task<T> Edit(T value);
    public Task Delete(int id);
}
public interface IDeviceService: ICrudService<Device>
{
    public Task<object> GetFullConfiguration(string UniqueDeviceId);
}

public interface IDeviceConfigService : ICrudService<DeviceConfig>
{
    public Task<List<DeviceConfig>> GetByDevice(int DeviceId);
    public Task<DeviceConfig> GetByDeviceConfigType(int DeviceConfigTypeId);
    Task<object?> Save(DeviceConfigView value);
}

public interface IDeviceDataService : ICrudService<DeviceData>
{
    public Task<List<DeviceData>> GetByDevice(int DeviceId);
    public Task<List<DeviceData>> GetByDeviceConfig(int DeviceConfigId);
}

public interface IImageService : ICrudService<Image> { }
public interface IUserService : ICrudService<User> { }