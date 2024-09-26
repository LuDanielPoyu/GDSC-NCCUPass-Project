using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.Register
{
    public class RegisterReq : IRequest<ResponseData<RegisterRes>>
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public List<string> Types { get; set; } = new List<string>();

        [Required]
        public List<BusinessHour> BusinessHour { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 內部人員驗證碼, 內部人員確認後才會激活帳戶(isActive)
        /// </summary>
        [Required]
        public string VerifyToken { get; set; } = null!;
    }


}

