using BusinessLogicLayer.Dtos.Products;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<ProductDetailedReadDto> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductReadDto>> GetProductsByFilter(ProductQuery productQuery, CancellationToken cancellationToken = default);
        Task<ProductDetailedReadDto> CreateProductAsync(ProductCreateDto productCreateDto, CancellationToken cancellationToken = default);
        Task<ProductDetailedReadDto> UpdateProductAsync(ProductUpdateDto productUpdateDto, CancellationToken cancellationToken = default);
        Task<ProductDetailedReadDto> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
