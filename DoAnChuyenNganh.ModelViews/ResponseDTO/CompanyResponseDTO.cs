namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class CompanyResponseDTO
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? LastUpdatedBy { get; set; }
        //public DateTimeOffset CreatedTime { get; set; }
        //public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
