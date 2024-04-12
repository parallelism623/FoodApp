using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.DataTransferObjects.Request.V1;

namespace FoodShop.Application.Services.Product
{

    public record CreateProductCommand(CreateProductRequest CreateProductRequest) : ICommand;
    public record DeleteProductCommand(Guid id) : ICommand;
    public record UpdateProductCommand(UpdateProductRequest UpdateProductRequest) : ICommand;
    
}
