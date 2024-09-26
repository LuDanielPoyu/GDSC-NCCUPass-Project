using System;
namespace NCCUPass.Application.Features.Restaurant.Queries.GetSearchRestaurant
{
    public class SearchRestaurantDTO
    {
        public string RestaurantId { get; set; } = "";
        public string Name { get; set; } = "";
        public List<BusinessHour> BusinessHour { get; set; } = null!;
        public string Location { get; set; } = "";
        public List<string> Types { get; set; } = null!;
    }
}
