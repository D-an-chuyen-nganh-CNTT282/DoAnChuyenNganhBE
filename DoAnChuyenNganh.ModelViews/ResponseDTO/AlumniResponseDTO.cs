namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public  class AlumniResponseDTO
    {
        public string Id { get; set; }
        public required Guid UserId { get; set; }
        public string AlumniName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string AlumniGender { get; set; }
        public string AlumniAddress { get; set; }
        public string AlumniEmail { get; set; }
        public string AlumniPhone { get; set; }
        public string AlumniMajor { get; set; }
        public string AlumniCourse { get; set; }
        public string PreviousClass { get; set; }
        public DateTime GraduationDay { get; set; }
        public required string LecturerId { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? LastUpdatedBy { get; set; }
        //public DateTimeOffset CreatedTime { get; set; }
        //public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
