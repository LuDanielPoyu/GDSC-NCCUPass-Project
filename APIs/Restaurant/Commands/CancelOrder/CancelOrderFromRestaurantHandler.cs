using System;
using NCCUPass.Database.Enums;

namespace NCCUPass.Application.Features.Restaurant.Commands.CancelOrder
{
    public class CancelOrderFromRestaurantHandler : BaseHandler, IRequestHandler<CancelOrderFromRestaurantReq, ResponseData<CancelOrderFromRestaurantRes>>
    {
        public CancelOrderFromRestaurantHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<CancelOrderFromRestaurantRes>> Handle(CancelOrderFromRestaurantReq request, CancellationToken cancellationToken)
        {
            // init
            var orderId = request.OrderId;
            var reason = request.Reason;
            var userId = new ObjectId(UserContext.UserId);

            // check
            var order = await UnitOfWork.Orders.FindOneAsync(_ => _.Id == new ObjectId(orderId));
            var status = await UnitOfWork.OrderStatuses.FindOneAsync(_ => _.Id == order.StatusId);

            if (order == null)
            {
                return new ResponseData<CancelOrderFromRestaurantRes>(404, false, "欲取消訂單不存在！", null);
            }
            if (status.Status == OrderStatusEnum.Done || status.Status == OrderStatusEnum.Invalid)
            {
                return new ResponseData<CancelOrderFromRestaurantRes>(403, false, "此訂單狀態不可取消！", null);
            }

            // operation
            var orderStatuses = await UnitOfWork.OrderStatuses.FindOneAsync(_ => _.Status == OrderStatusEnum.Invalid); // 取消是不成立
            order.StatusId = orderStatuses.Id;

            var user = await UnitOfWork.Users.FindOneAsync(_ => _.Id == userId);
            var restaurant = await UnitOfWork.Restaurants.FindOneAsync(_ => _.Id == userId);
            var fromType = (user != null) ? UserRoleEnum.NCCUStudent : ((restaurant != null) ? UserRoleEnum.Restaurant : 0);

            var cancelOrderRecord = new CancelOrderRecords()
            {
                FromType = fromType,
                FromId = userId,
                OrderId = order.Id,
                Reason = reason
            };

            UnitOfWork.StartTransaction();
            await UnitOfWork.Orders.ReplaceOneAsync(order);
            await UnitOfWork.CancelOrderRecords.InsertOneAsync(cancelOrderRecord);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<CancelOrderFromRestaurantRes>(200, true, "成功取消訂單！", new CancelOrderFromRestaurantRes
            {
                OrderId = orderId,
                OrderStatus = orderStatuses.Status.ToString(),
                Reason = reason
            });
        }
    }
}

