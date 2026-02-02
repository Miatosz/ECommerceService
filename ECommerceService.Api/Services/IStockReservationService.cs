namespace ECommerceService.Api.Services
{
    public interface IStockReservationService
    {

        Task ReserveAsync(int productId, int quantity);
        Task RestoreAsync(int productId, int quantity);
    }
}