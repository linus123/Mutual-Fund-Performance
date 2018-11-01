using System;
using System.Collections.Generic;
using System.Text;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.SharedKernel
{
    public interface IMutualFundDataGateway
    {
         MutualFundDto[] GetAll();
        void Insert(MutualFundDto[] data);

        void DeleteAll();
    }
}
