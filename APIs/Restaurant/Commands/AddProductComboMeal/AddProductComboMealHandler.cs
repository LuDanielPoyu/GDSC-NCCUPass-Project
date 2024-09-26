using System;
using NCCUPass.Application.Features.Restaurant.Commands.AddProductFood;
using NCCUPass.Application.Features.TakeAwayService.Commands.AddItemToBag;
using NCCUPass.Database.Entities;

namespace NCCUPass.Application.Features.Restaurant.Commands.AddProductComboMeal
{
	public class AddProductComboMealHandler : BaseHandler, IRequestHandler<AddProductComboMealReq, ResponseData<AddProductComboMealRes>>
	{
		public AddProductComboMealHandler(IServiceProvider provider) : base(provider)
		{
		}

        public async Task<ResponseData<AddProductComboMealRes>> Handle(AddProductComboMealReq request, CancellationToken cancellationToken)
        {
            // init
            var comboMeals = request.ComboMeals;
            var restaurantId = new ObjectId(UserContext.UserId);

            // check
            foreach (var comboMeal in comboMeals)
            {
                foreach (var id in comboMeal.FoodIds)
                {
                    var foodId = new ObjectId(id);
                    var food = await UnitOfWork.Foods.FindOneAsync(_ => _.Id == foodId);
                    if (food == null)
                    {
                        return new ResponseData<AddProductComboMealRes>(404, false, $"套餐{comboMeal.Name}中, 餐點{foodId}不存在！", null);
                    }
                }
            }

            // operation
            var combos = new List<ComboMeals>();
            var foodCombosList = new List<List<FoodCombos>>();
            var products = new List<Products>();
            foreach (var comboMeal in comboMeals)
            {
                var combo = new ComboMeals()
                {
                    Name = comboMeal.Name
                };

                var foodCombos = new List<FoodCombos>();
                foreach(var foodId in comboMeal.FoodIds)
                {
                    var foodCombo = new FoodCombos()
                    {
                         ComboId = combo.Id,
                         FoodId = new ObjectId(foodId)
                    };
                    foodCombos.Add(foodCombo);
                }

                var product = new Products()
                {
                    RestaurantID = restaurantId,
                    FoodID = null,
                    ComboID = combo.Id,
                    Category = comboMeal.Category,
                    Price = comboMeal.Price
                };

                combos.Add(combo);
                foodCombosList.Add(foodCombos);
                products.Add(product);
            }


            UnitOfWork.StartTransaction();
            await UnitOfWork.ComboMeals.InsertManyAsync(combos);
            foreach(var foodCombos in foodCombosList)
            {
                await UnitOfWork.FoodCombos.InsertManyAsync(foodCombos);
            }
            await UnitOfWork.Products.InsertManyAsync(products);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<AddProductComboMealRes>(200, true, $"成功新增{products.Count()}筆套餐商品！", null);
        }
    }
}

