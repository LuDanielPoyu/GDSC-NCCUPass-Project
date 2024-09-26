using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.CancelOrder
{
    public class CancelOrderFromRestaurantRes
    {
        public string OrderId { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string Reason { get; set; } = "";
    }
}

