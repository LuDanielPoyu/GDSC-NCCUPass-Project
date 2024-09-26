using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.Register
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterReq, Restaurants>();

            CreateMap<RegisterReq, RestaurantCredentials>();
        }
    }
}

