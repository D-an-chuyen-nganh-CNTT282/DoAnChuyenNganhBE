
namespace DoAnChuyenNganh.ModelViews.ActivitiesModelViews
{
    public class ActivitiesModelView
    {
        public enum EventType
        {
            Student,
            Lecturer,
            Business,
            Alumni
        }
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public EventType EventTypes { get; set; }

        public string? Description { get; set; }
    }
}
