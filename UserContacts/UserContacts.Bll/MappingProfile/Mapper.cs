using AutoMapper;
using System.Text.RegularExpressions;
using UserContacts.Bll.Dtos;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.MappingProfile;

public class Mapper : Profile
{ 
    public Mapper()
    {
        CreateMap<UserCreateDto, User>().ReverseMap();
        CreateMap<UserGetDto, User>().ReverseMap();
        CreateMap<ContactCreateDto, Contact>().ReverseMap();
        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<RoleCreateDto, UserRole>().ReverseMap();
    }
}
