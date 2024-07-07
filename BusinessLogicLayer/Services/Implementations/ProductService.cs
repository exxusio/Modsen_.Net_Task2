using AutoMapper;
using BusinessLogicLayer.Dtos.Products;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Algorithms;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDetailedReadDto> CreateProductAsync(ProductCreateDto productCreateDto, CancellationToken cancellationToken = default)
        {
            if (productCreateDto == null)
            {
                throw new ArgumentNullException(nameof(productCreateDto));
            }

            var productRepository = _unitOfWork.GetRepository<Product>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();

            var product = _mapper.Map<Product>(productCreateDto);

            var existingCategory = await categoryRepository.GetByIdAsync(productCreateDto.CategoryId, cancellationToken);
            if (existingCategory == null)
            {
                throw new NotFoundException($"Category not found with id: {productCreateDto.CategoryId}");
            }

            product.Category = existingCategory;

            await productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductDetailedReadDto>(product);
        }

        public async Task<ProductDetailedReadDto> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var productRepository = _unitOfWork.GetRepository<Product>();

            var product = await productRepository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException($"Product not found with id: {id}");
            }

            productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductDetailedReadDto>(product);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            var productRepository = _unitOfWork.GetRepository<Product>();

            var products = await productRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<ProductDetailedReadDto> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var productRepository = _unitOfWork.GetRepository<Product>();

            var product = await productRepository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException($"Product not found with id: {id}");
            }

            return _mapper.Map<ProductDetailedReadDto>(product);
        }

        public async Task<ProductDetailedReadDto> UpdateProductAsync(ProductUpdateDto productUpdateDto, CancellationToken cancellationToken = default)
        {
            if (productUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(productUpdateDto));
            }

            var productRepository = _unitOfWork.GetRepository<Product>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();

            var existingProduct = await productRepository.GetByIdAsync(productUpdateDto.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product not found with id: {productUpdateDto.Id}");
            }

            var existingCategory = await categoryRepository.GetByIdAsync(productUpdateDto.CategoryId, cancellationToken);
            if (existingCategory == null)
            {
                throw new NotFoundException($"Category not found with id: {productUpdateDto.CategoryId}");
            }

            existingProduct.Category = existingCategory;

            var newProduct = _mapper.Map(productUpdateDto, existingProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductDetailedReadDto>(newProduct);
        }

        public async Task<IEnumerable<ProductReadDto>> GetProductsByFilter(ProductQuery productQuery, CancellationToken cancellationToken = default)
        {
            Expression<Func<Product, bool>> predicate = product => true;

            if (!string.IsNullOrEmpty(productQuery.Name))
            {
                predicate = predicate.And(product => product.Name.Contains(productQuery.Name));
            }

            if (!string.IsNullOrEmpty(productQuery.Description))
            {
                predicate = predicate.And(product => product.Description.Contains(productQuery.Description));
            }

            if (productQuery.MaxPrice > 0)
            {
                predicate = predicate.And(product => product.Price < productQuery.MaxPrice);
            }

            if (productQuery.MinPrice < 0)
            {
                predicate = predicate.And(product => product.Price > productQuery.MinPrice);
            }

            if (productQuery.CategoryId != Guid.Empty)
            {
                predicate = predicate.And(product => product.CategoryId == productQuery.CategoryId);
            }

            var productRepository = _unitOfWork.GetRepository<Product>();

            var products = await productRepository.GetByPredicateAsync(predicate, cancellationToken);

            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }
    }
}
