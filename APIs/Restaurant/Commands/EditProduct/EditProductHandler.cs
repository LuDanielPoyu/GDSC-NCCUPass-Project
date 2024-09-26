using System;
using NCCUPass.Application.Features.Task.NormalTask.Commands.EditNormalTask;
using NCCUPass.Database.Entities;

namespace NCCUPass.Application.Features.RestaurantManage.Commands.EditProduct
{
	public class EditProductHandler: BaseHandler,IRequestHandler<EditProductReq, ResponseData<EditProductRes>>
	{
		public EditProductHandler(IServiceProvider provider) : base(provider)
		{
		}

        public async Task<ResponseData<EditProductRes>> Handle(EditProductReq request, CancellationToken cancellationToken)
        {
            // init
            var originalProducts= await UnitOfWork.Products.FindOneAsync(_ => _.Id == new ObjectId(request.ProductId));
            var restaurantId = new ObjectId(UserContext.UserId);

            // check
            if (originalProducts.RestaurantID != restaurantId){
                return new ResponseData<EditProductRes>(403, false, "並非屬於此餐廳！", null);
            }
            var originalFoods = await UnitOfWork.Foods.FindOneAsync(_ => _.Id == originalProducts.FoodID);
            if (originalProducts.FoodID != null){
                originalFoods.Name = request.newName;
            }
            var originalComboMeals = await UnitOfWork.ComboMeals.FindOneAsync(_ => _.Id == originalProducts.ComboID);
            if (originalProducts.ComboID != null){
                originalComboMeals.Name = request.newName;
            }

            // operation
            originalProducts.Price = request.newPrice;
            originalProducts.Category = request.newCategory;

            UnitOfWork.StartTransaction();
            await UnitOfWork.Products.ReplaceOneAsync(originalProducts);

            // edit Foods
            if (originalProducts.FoodID != null)
            {
                await UnitOfWork.Foods.ReplaceOneAsync(originalFoods);
            } else if(originalProducts.ComboID != null)
            {
                // edit Combomeals
                await UnitOfWork.ComboMeals.ReplaceOneAsync(originalComboMeals);
            }
            await UnitOfWork.SaveChangesAsync();
            return new ResponseData<EditProductRes>(200,true, "成功編輯商品！", null);
        }
    }
}

