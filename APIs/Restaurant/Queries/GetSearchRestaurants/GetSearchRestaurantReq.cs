using System;

namespace NCCUPass.Application.Features.Restaurant.Queries.GetSearchRestaurant
{
    public class GetSearchRestaurantReq : IRequest<ResponseData<GetSearchRestaurantRes>>
    {
        [Required]
        public string Keyword { get; set; } = null!;
    }
}
