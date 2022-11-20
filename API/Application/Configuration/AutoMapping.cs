using Application.Models;
using Application.Models.View;
using AutoMapper;

namespace Application.Configuration;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Device, DeviceView>(); // means you want to map from User to UserDTO
        CreateMap<DeviceView, Device>(); // means you want to map from User to UserDTO
    }
}