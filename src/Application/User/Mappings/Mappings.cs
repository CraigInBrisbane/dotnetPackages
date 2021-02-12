using Application.User.Handlers;
using AutoMapper;
using Domain.Requests;

namespace Application.User.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<RegisterUserRequest, RegisterUserQuery>();
        }
    }
}
