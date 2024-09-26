using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.SignIn
{
    public class SignInRestaurantReq : IRequest<ResponseData<SignInRestaurantRes>>
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

