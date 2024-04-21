using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Contract.Abstraction.Message;

namespace FoodShop.Application.Products.ProductCommand
{

    public record CreateProductCommand(Guid Id, CreateProductRequest CreateProductRequest) : ICommand;
    public record DeleteProductCommand(Guid Id) : ICommand;
    public record UpdateProductCommand(Guid Id, UpdateProductRequest UpdateProductRequest) : ICommand;

}
