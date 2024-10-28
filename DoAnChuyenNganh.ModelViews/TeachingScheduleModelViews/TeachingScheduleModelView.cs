namespace DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews
{
    public class TeachingScheduleModelView
    {
        public Guid UserId { get; set; }
        public required string LecturerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Subject { get; set; }
        public required string Location { get; set; }
        public required string ClassPeriod { get; set; }
    }
}
