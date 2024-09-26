using System;
namespace NCCUPass.Application.Features.Restaurant.Commands.AcceptOrder
{
	public class AcceptOrderReq : IRequest<ResponseData<AcceptOrderRes>>
	{
		[Required]
		public string OrderId { get; set; } = "";
	}
}

