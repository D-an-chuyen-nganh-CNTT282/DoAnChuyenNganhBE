using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class IncomingDocumentResponseDTO
    {
        public string Id {  get; set; }
        public string IncomingDocumentTitle { get; set; }
        public string IncomingDocumentContent { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DepartmentId { get; set; }
        public required string FileScanUrl { get; set; }
        public required Guid UserId { get; set; }
    }
}
