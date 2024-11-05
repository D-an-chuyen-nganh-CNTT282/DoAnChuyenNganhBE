namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class LecturerResponseDTO
    {
        public string Id { get; set; }
        public string LecturerName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string LecturerGender { get; set; }
        public string LecturerEmail { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerAddress { get; set; }
        public string Expertise { get; set; }
        public string? PersonalWebsiteLink { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? LastUpdatedBy { get; set; }
        //public DateTimeOffset CreatedTime { get; set; }
        //public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
