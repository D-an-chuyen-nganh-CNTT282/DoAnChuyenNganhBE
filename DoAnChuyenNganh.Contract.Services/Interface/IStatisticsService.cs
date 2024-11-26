using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IStatisticsService
    {
        Task<Dictionary<string, int>> GetEntityCountsAsync();
        Task<BasePaginatedList<ActivitiesResponseDTO>> GetUpcomingActivitiesAsync(int pageIndex, int pageSize);

    }
}
