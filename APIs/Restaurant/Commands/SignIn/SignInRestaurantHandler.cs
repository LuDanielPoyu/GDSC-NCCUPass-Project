using System;
using NCCUPass.Identity.Models;
using System.Security.Claims;
using NCCUPass.Identity.Services;

namespace NCCUPass.Application.Features.Restaurant.Commands.SignIn
{
    public class SignInRestaurantHandler : BaseHandler, IRequestHandler<SignInRestaurantReq, ResponseData<SignInRestaurantRes>>
    {
        [Inject] private IJwtService _jwtService = null!;

        public SignInRestaurantHandler(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public async Task<ResponseData<SignInRestaurantRes>> Handle(SignInRestaurantReq request, CancellationToken cancellationToken)
        {
            // 一樣會存到UserContext

            // init
            string password = request.Password;
            string email = request.Email;

            // check
            var credential = await UnitOfWork.RestaurantCredentials.FindOneAsync(_ => _.Email.Equals(email));
            if (credential == null)
            {
                return new ResponseData<SignInRestaurantRes>(404, false, "找不到餐廳使用者！", null);
            }

            bool isValidate = PasswordService.ValidatePassword(password, credential.Password);
            if (!isValidate)
            {
                return new ResponseData<SignInRestaurantRes>(403, false, "密碼錯誤！", null);
            }

            var tryRes = await UnitOfWork.Restaurants.FindOneAsync(_ => _.Id == credential.RestaurantId);
            if (tryRes == null)
            {
                return new ResponseData<SignInRestaurantRes>(404, false, "餐廳使用者資料不存在！", null);
            }
            bool isActive = tryRes.IsActive;
            if (!isActive)
            {
                return new ResponseData<SignInRestaurantRes>(403, false, "餐廳使用者身份尚未開通！", null);
            }

            // operation
            // if validate, generate JWT and return to user
            var role = ((UserRoleEnum)UserRoleEnum.Restaurant).ToString();
            string accessToken = _jwtService.GenerateJwtToken(new Claim[]
            {
                new Claim("Email", email),
                new Claim("Role", role),
                new Claim("UserId", credential.RestaurantId.ToString())
            }, DateTime.Now);

            // generate refresh token once user login and store in DB.
            // Once it is used, assign a new one in DB
            string refreshToken = _jwtService.GenerateDisposableRefreshToken(new Claim[]
            {
                new Claim("Email", email),
                new Claim("Role", role),
                new Claim("UserId", credential.RestaurantId.ToString()),
                new Claim("SignDate", DateTime.Now.ToString())
            });
            credential.RefreshToken = refreshToken;

            UnitOfWork.StartTransaction();
            await UnitOfWork.RestaurantCredentials.ReplaceOneAsync(credential);
            await UnitOfWork.SaveChangesAsync();

            var res = Mapper.Map<SignInRestaurantRes>(credential);
            res.JWT = new JWTModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            res.Role = role;

            return new ResponseData<SignInRestaurantRes>(200, true, "登入成功！", res);
        }
    }
}

