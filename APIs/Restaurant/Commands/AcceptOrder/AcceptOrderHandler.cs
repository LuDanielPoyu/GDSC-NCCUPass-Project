using System;

namespace NCCUPass.Application.Features.Restaurant.Commands.AcceptOrder
{
	public class AcceptOrderHandler : BaseHandler, IRequestHandler<AcceptOrderReq, ResponseData<AcceptOrderRes>>
	{
		public AcceptOrderHandler(IServiceProvider provider) : base(provider)
		{
		}

        public async Task<ResponseData<AcceptOrderRes>> Handle(AcceptOrderReq request, CancellationToken cancellationToken)
        {
            // init
            var restId = new ObjectId(UserContext.UserId);
            var orderId = request.OrderId;

            // check
            var order = await UnitOfWork.Orders.FindOneAsync(_ => _.Id == new ObjectId(orderId));
            if (order == null)
            {
                return new ResponseData<AcceptOrderRes>(404, false, "欲接受訂單不存在！", null);
            }
            if(order.RestaurantId != restId)
            {
                return new ResponseData<AcceptOrderRes>(403, false, "您不是此訂單的餐廳！", null);
            }

            var status = await UnitOfWork.OrderStatuses.FindOneAsync(_ => _.Id == order.StatusId);
            if (status.Status != OrderStatusEnum.ToBeConfirmed)
            {
                return new ResponseData<AcceptOrderRes>(403, false, "此訂單狀態不可被接受！", null);
            }

            // operation
            var orderStatuses = await UnitOfWork.OrderStatuses.FindOneAsync(_ => _.Status == OrderStatusEnum.Confirmed); 
            order.StatusId = orderStatuses.Id;

            UnitOfWork.StartTransaction();
            await UnitOfWork.Orders.ReplaceOneAsync(order);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<AcceptOrderRes>(200, true, "成功接受訂單！", new AcceptOrderRes
            {
                OrderId = orderId,
                OrderStatus = orderStatuses.Status.ToString()
            });
        }
    }
}