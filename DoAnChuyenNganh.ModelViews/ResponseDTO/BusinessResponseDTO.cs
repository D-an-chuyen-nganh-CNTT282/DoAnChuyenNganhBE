
namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class BusinessResponseDTO
    {
        public string Id { get; set; }
        public string LecturerId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessEmail { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
