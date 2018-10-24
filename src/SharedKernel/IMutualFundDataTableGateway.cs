using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.SharedKernel
{
    public interface IMutualFundDataTableGateway
    {
        MutualFundDto[] GetAll();
    }
}
