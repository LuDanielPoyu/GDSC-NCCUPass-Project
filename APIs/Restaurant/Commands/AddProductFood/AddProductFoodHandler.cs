using System;
using System.Xml.Linq;

namespace NCCUPass.Application.Features.Restaurant.Commands.AddProductFood
{
    public class AddProductFoodHandler : BaseHandler, IRequestHandler<AddProductFoodReq, ResponseData<AddProductFoodRes>>
    {
        public AddProductFoodHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<AddProductFoodRes>> Handle(AddProductFoodReq request, CancellationToken cancellationToken)
        {
            // init
            var productFoods = request.ProductFoods;
            var restaurantId = new ObjectId(UserContext.UserId);

            // check

            // operation
            var foods = new List<Foods>();
            var products = new List<Products>();
            foreach(var productFood in productFoods)
            {
                var food = new Foods()
                {
                    Name = productFood.Name
                };

                var product = new Products()
                {
                    RestaurantID = restaurantId,
                    FoodID = food.Id,
                    ComboID = null,
                    Category = productFood.Category,
                    Price = productFood.Price
                };

                foods.Add(food);
                products.Add(product);
            }
            

            UnitOfWork.StartTransaction();
            await UnitOfWork.Foods.InsertManyAsync(foods);
            await UnitOfWork.Products.InsertManyAsync(products);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<AddProductFoodRes>(200, true, $"成功新增{products.Count()}筆餐點！", null);
        }
    }
}

