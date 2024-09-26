using System;


namespace NCCUPass.Application.Features.Restaurant.Commands.AddProductComboMeal
{
	public class AddProductComboMealReq : IRequest<ResponseData<AddProductComboMealRes>>
	{
        [Required]
        public List<AddProductComboMealReqDTO> ComboMeals { get; set; } = new List<AddProductComboMealReqDTO>();
    }

    public class AddProductComboMealReqDTO
    {
        [Required]
        public List<string> FoodIds { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!; // product(combo meal) category

        [Required]
        public float Price { get; set; } 

    }
}

