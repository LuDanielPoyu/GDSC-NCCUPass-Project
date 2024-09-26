using System;
namespace NCCUPass.Application.Features.Restaurant.Queries.GetAllProducts
{
	public class ProductDTO
	{
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string Category { get; set; } = "";
        public int Price { get; set; }
    }
}

