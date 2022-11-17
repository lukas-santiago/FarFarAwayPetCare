using Application.Models;
using AutoMapper;

namespace Application.Configuration;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        //CreateMap<User, UserDTO>(); // means you want to map from User to UserDTO
    }
}