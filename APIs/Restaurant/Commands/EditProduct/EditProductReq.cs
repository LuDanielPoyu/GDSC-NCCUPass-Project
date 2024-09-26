using System;
namespace NCCUPass.Application.Features.RestaurantManage.Commands.EditProduct
{
	public class EditProductReq : IRequest<ResponseData<EditProductRes>>
	{
        [Required]
        public string newName { get; set; } = null!;

        [Required]
        public string ProductId { get; set; } = null!;

        [Required]
        public string newCategory { get; set; } = null!;

        [Required]
        public int newPrice { get; set; } 

    }
}

