namespace NCCUPass.Application.Features.Restaurant.Queries.GetSearchRestaurant
{
    public class GetSearchRestaurantHandler : BaseHandler, IRequestHandler<GetSearchRestaurantReq, ResponseData<GetSearchRestaurantRes>>
    {
        public GetSearchRestaurantHandler(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<ResponseData<GetSearchRestaurantRes>> Handle(GetSearchRestaurantReq request, CancellationToken cancellationToken)
        {
            // init
            var keyword = request.Keyword;

            // check
            // operation
            var searchrestaurantsdto = await UnitOfWork.Restaurants.GetSearchRestaurantAsync(keyword);
            var resdto = new List<SearchRestaurantDTO>();
            Mapper.Map<List<Restaurants>, List<SearchRestaurantDTO>>(searchrestaurantsdto, resdto);

            return new ResponseData<GetSearchRestaurantRes>(200, true, "成功取得餐廳資訊！", new GetSearchRestaurantRes()
            {
                Restaurants = resdto
            });
        }
    }
}
