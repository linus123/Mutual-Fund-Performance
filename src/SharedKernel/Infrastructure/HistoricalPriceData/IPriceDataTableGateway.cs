namespace MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData
{
    public interface IPriceDataTableGateway
    {
        PriceDto[] GetAll();
        void Insert(PriceDto[] dtos);
        void DeleteAll();
    }
}