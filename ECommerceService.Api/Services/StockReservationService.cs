using ECommerceService.Api.Exceptions;
using ECommerceService.Api.Repositories;

namespace ECommerceService.Api.Services
{
    public class StockReservationService : IStockReservationService
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<StockReservationService> _logger;

        public StockReservationService(IProductRepository repo, ILogger<StockReservationService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task ReserveAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.", nameof(quantity));

            // Sprawdzenie czy produkt istnieje
            var stock = await _repo.GetStockByIdAsync(productId);
            if (stock == null)
                throw new ProductNotFoundException(productId);

            if (stock < quantity)
                throw new InsufficientStockException(productId, quantity, stock.Value);

            // Atomowa rezerwacja
            var success = await _repo.TryDecreaseStockAsync(productId, quantity);
            if (!success)
                throw new InsufficientStockException(productId, quantity, stock.Value);

            _logger.LogInformation(
                "Stock reserved: ProductId={ProductId}, Quantity={Quantity}",
                productId, quantity);
        }

        public async Task RestoreAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.", nameof(quantity));

            var product = await _repo.GetByIdAsync(productId)
                ?? throw new ProductNotFoundException(productId);

            product.UpdateStock(product.Stock + quantity);
            await _repo.UpdateAsync(product);

            _logger.LogInformation(
                "Stock restored: ProductId={ProductId}, Quantity={Quantity}",
                productId, quantity);
        }
    }
}