using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.CancelOrder
{
    public class CancelOrderFromRestaurantReq : IRequest<ResponseData<CancelOrderFromRestaurantRes>>
    {
        [Required]
        public string OrderId { get; set; } = "";

        [Required]
        public string Reason { get; set; } = "";
    }
}

