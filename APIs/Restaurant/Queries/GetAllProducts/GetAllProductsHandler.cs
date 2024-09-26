using System;
using NCCUPass.Database.Entities;


namespace NCCUPass.Application.Features.Restaurant.Queries.GetAllProducts
{
	public class GetAllProductsHandler : BaseHandler , IRequestHandler<GetAllProductsReq, ResponseData<GetAllProductsRes>>
	{
        public GetAllProductsHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<GetAllProductsRes>> Handle(GetAllProductsReq requset , CancellationToken cancellationToken)
        {
            // init
            var restaurantId = new ObjectId(UserContext.UserId);
            var resDto = new List<ProductDTO>();

            // check


            // operation
            var products = await UnitOfWork.Products.FindByAsync(_ => _.RestaurantID == restaurantId);
            foreach (var p in products)
            {
                if(p.FoodID == null)
                {
                    var comboM = await UnitOfWork.ComboMeals.FindOneAsync(_ => _.Id == p.ComboID);
                    var dto = new ProductDTO()
                    {
                        ProductId = p.Id.ToString(),
                        ProductName = comboM.Name,
                        Category = "ComboMeal",
                        Price = (int)p.Price
                    };
                    resDto.Add(dto);
                }
                else
                {
                    var Food = await UnitOfWork.Foods.FindOneAsync(_ => _.Id == p.FoodID);
                    var dto = new ProductDTO()
                    {
                        ProductId = p.Id.ToString(),
                        ProductName = Food.Name,
                        Category = "Food",
                        Price = (int)p.Price
                    };
                    resDto.Add(dto);
                }
                
            }

            return new ResponseData<GetAllProductsRes>(200, true, "成功取得所有商品！", new GetAllProductsRes()
            {
                Products = resDto
            });
        }
    }
}

