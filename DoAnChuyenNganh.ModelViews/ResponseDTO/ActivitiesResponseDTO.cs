
namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class ActivitiesResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string EventTypes { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
