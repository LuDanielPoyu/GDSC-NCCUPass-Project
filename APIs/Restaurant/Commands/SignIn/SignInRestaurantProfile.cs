using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.SignIn
{
    public class SignInRestaurantProfile : Profile
    {
        public SignInRestaurantProfile()
        {
            CreateMap<RestaurantCredentials, SignInRestaurantRes>()
              .ForMember(
                  dest => dest.UserId,
                  opt => opt.MapFrom(from => from.RestaurantId)
              );
        }
    }
}

