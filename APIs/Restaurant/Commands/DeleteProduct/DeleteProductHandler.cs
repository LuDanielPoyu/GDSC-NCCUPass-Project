using System;
using MediatR;
using MongoDB.Bson;
using NCCUPass.Application.Models;
using NCCUPass.Application.Features.Restaurant.Commands.DeleteProduct;
using NCCUPass.Application.Features.Task.NormalTask.Commands.EditNormalTask;
using NCCUPass.Database.Entities;

namespace NCCUPass.Application.Features.RestaurantManage.Commands.DeleteProduct
{
    public class DeleteProductHandler : BaseHandler, IRequestHandler<DeleteProductReq, ResponseData<DeleteProductRes>>
    {
        public DeleteProductHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<DeleteProductRes>> Handle(DeleteProductReq request, CancellationToken cancellationToken)
        {
            // init
            var productId = new ObjectId(request.ProductId);

            var tryProduct = await UnitOfWork.Products.FindOneAsync(_ => _.Id == productId);
            if (tryProduct == null)
            {
                return new ResponseData<DeleteProductRes>(404, true, "商品不存在!", null);
            }

            var restaurantId = new ObjectId(UserContext.UserId);

            //check
            if (tryProduct.RestaurantID != restaurantId)
            {
                return new ResponseData<DeleteProductRes>(403, false, "並非屬於此餐廳!", null);
            }        


            //operation
            UnitOfWork.StartTransaction();

            await UnitOfWork.Products.DeleteProductRelatedDataAsync(tryProduct);

            await UnitOfWork.Products.DeleteOneAsync(_ => _.Id == productId);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<DeleteProductRes>(200, true, "成功刪除商品!", null);

        }
    }
}
