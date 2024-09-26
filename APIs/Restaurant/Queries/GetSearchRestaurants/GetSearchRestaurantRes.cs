using System;
using NCCUPass.Application.Features.Restaurant.Queries.GetAllProducts;

namespace NCCUPass.Application.Features.Restaurant.Queries.GetSearchRestaurant
{
    public class GetSearchRestaurantRes
    {
        public List<SearchRestaurantDTO> Restaurants { get; set; } = new List<SearchRestaurantDTO>();
    }
}
