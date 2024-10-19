using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class ExtracurricularActivitiesReponseDTO
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string ActivitiesId { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
