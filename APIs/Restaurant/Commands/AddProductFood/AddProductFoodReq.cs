using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.AddProductFood
{
    public class AddProductFoodReq : IRequest<ResponseData<AddProductFoodRes>>
    {
        [Required]
        public List<AddProductFoodReqDTO> ProductFoods { get; set; } = new List<AddProductFoodReqDTO>();
    }

    public class AddProductFoodReqDTO
    {
        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public float Price { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}

