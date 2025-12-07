using AutoMapper;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Application.DTOs.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity -> Output DTO
        CreateMap<User, UserDto>();

        // Input DTO -> Entity
        CreateMap<CreateUserDto, User>();
        //CreateMap<UpdateUserDto, User>();
    }
}
