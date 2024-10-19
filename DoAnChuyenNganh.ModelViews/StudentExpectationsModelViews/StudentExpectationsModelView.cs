using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews
{
    public class StudentExpectationsModelView
    {
        public string StudentId { get; set; }

        public string RequestCategory { get; set; }

        public Guid UserId { get; set; }

        public string ProcessingStatus { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public string FileScanUrl { get; set; }
    }
}
