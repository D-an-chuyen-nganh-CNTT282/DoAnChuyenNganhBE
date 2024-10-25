namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class AlumniCompanyResponseDTO
    {
        public string Id { get; set; }
        public string AlumniId { get; set; }
        public string CompanyId { get; set; }
        public string Duty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
