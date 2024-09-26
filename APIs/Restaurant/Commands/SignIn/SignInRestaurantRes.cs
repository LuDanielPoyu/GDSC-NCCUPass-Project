using System;
using NCCUPass.Identity.Models;

namespace NCCUPass.Application.Features.Restaurant.Commands.SignIn
{
    public class SignInRestaurantRes
    {
        public JWTModel? JWT { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; } 
        public string? UserId { get; set; } 
    }
}

