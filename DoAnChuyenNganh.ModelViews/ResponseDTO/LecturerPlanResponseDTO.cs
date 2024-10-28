namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class LecturerPlanResponseDTO
    {
        public string Id { get; set; }
        public string LecturerId { get; set; }
        public required string PlanContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
