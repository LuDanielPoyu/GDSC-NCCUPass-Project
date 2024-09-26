using NCCUPass.Application.Features.Restaurant.Commands.DeleteProduct;
using System;

namespace NCCUPass.Application.Features.RestaurantManage.Commands.DeleteProduct
{
    public class DeleteProductReq : IRequest<ResponseData<DeleteProductRes>>
    {
        [Required]
        public string ProductId { get; set; } = null!;
    }
}
