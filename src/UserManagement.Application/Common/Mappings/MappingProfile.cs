using AutoMapper;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;

namespace UserManagement.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserCommandModel, Domain.Entities.User>();

        CreateMap<UpdateUserCommandModel, Domain.Entities.User>();

        CreateMap<Domain.Entities.User, UserDto>();

        CreateMap<Domain.Entities.User, AuthenticateUserResponse>();

        CreateMap<Domain.Entities.Role, RoleDto>();    
    }
}