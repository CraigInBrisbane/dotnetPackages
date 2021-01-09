using System;
using System.Collections.Generic;
using System.Text;
using Api.Business.User.Handlers;
using Api.Core.Contracts.Requests;
using AutoMapper;

namespace Api.Business.User.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<RegisterUserRequest, RegisterUserQuery>();
        }
    }
}
