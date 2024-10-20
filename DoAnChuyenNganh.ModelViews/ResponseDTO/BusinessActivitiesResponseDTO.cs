
namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class BusinessActivitiesResponseDTO
    {
        public string Id { get; set; }
        public string BusinessId { get; set; }
        public string ActivitiesId { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
