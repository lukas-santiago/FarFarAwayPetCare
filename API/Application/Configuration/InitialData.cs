using Microsoft.EntityFrameworkCore;

namespace Application.Configuration;
public class InitialDataGenerator
{
    public InitialDataGenerator(ApiContext context)
    {
        if (!context.User.Any())
            context.User.Add(new Models.User()
            {
                Username = "string",
                Password = "string",
                Role = "admin",
            });

        if (!context.Device.Any())
            context.Device.Add(new Models.Device()
            {
                Nome = "Aquário 1",
                UniqueDeviceId = "fd8d872d-3045-47c8-8b24-9db2d4807b7c",
                User = "string",
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

        if (!context.DeviceConfigType.Any())
            context.DeviceConfigType.AddRange(
                new Models.DeviceConfigType() { Nome = "pH" },
                new Models.DeviceConfigType() { Nome = "Temperatura" },
                new Models.DeviceConfigType() { Nome = "Amônia" },
                new Models.DeviceConfigType() { Nome = "Imagem" },
                new Models.DeviceConfigType() { Nome = "Iluminação" },
                new Models.DeviceConfigType() { Nome = "Tomada" },
                new Models.DeviceConfigType() { Nome = "Alimentação" }
            );

        context.SaveChanges();
    }
}