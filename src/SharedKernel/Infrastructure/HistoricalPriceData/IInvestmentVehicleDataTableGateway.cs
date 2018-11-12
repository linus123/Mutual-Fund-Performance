namespace MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData
{
    public interface IInvestmentVehicleDataTableGateway
    {
        InvestmentVehicleDto[] GetAll();
        void Insert(InvestmentVehicleDto[] dtos);
        void DeleteAll();
    }
}