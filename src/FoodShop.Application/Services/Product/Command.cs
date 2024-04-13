using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.DataTransferObjects.Request.V1;

namespace FoodShop.Application.Services.Product
{

    public record CreateProductCommand(Guid Id, CreateProductRequest CreateProductRequest) : ICommand;
    public record DeleteProductCommand(Guid Id) : ICommand;
    public record UpdateProductCommand(Guid Id, UpdateProductRequest UpdateProductRequest) : ICommand;
    
}
