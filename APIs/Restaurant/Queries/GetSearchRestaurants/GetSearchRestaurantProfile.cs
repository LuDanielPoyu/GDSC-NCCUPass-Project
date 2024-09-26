using System;

namespace NCCUPass.Application.Features.Restaurant.Queries.GetSearchRestaurant
{
    public class GetSearchRestaurantProfile : Profile
    {
        public GetSearchRestaurantProfile()
        {
            CreateMap<Restaurants, SearchRestaurantDTO>()
                .ForMember(
                    dest => dest.RestaurantId,
                    opt => opt.MapFrom(from => from.Id.ToString())
                );
        }
    }
}
