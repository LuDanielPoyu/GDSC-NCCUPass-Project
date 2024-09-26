using System;
using System.Net.Mail;
using NCCUPass.Identity.Services;

namespace NCCUPass.Application.Features.Restaurant.Commands.Register
{
    public class RegisterHandler : BaseHandler, IRequestHandler<RegisterReq, ResponseData<RegisterRes>>
    {
        public RegisterHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<RegisterRes>> Handle(RegisterReq request, CancellationToken cancellationToken)
        {
            // init
            var restaurant = Mapper.Map<RegisterReq, Restaurants>(request);
            var restaurantCredential = Mapper.Map<RegisterReq, RestaurantCredentials>(request);

            // check
            if(!IsValidEmail(restaurantCredential.Email))
            {
                return new ResponseData<RegisterRes>(403, false, "不合法的Email！", null);
            }
            if((await UnitOfWork.RestaurantCredentials.FindOneAsync(_ => _.Email.Equals(restaurantCredential.Email))) != null)
            {
                return new ResponseData<RegisterRes>(403, false, "此Email已被使用！", null);
            }

            // operation
            restaurantCredential.RestaurantId = restaurant.Id;

            string salt = PasswordService.GenerateSalt();
            string hasedPassword = PasswordService.HashPassword(restaurantCredential.Password, salt);
            restaurantCredential.Password = hasedPassword;
            restaurantCredential.Salt = salt;

            UnitOfWork.StartTransaction();
            await UnitOfWork.Restaurants.InsertOneAsync(restaurant);
            await UnitOfWork.RestaurantCredentials.InsertOneAsync(restaurantCredential);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<RegisterRes>(200, true, "註冊商家成功！請通知NCCUPass內部人員開通商家帳號！", null);
        }

        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

