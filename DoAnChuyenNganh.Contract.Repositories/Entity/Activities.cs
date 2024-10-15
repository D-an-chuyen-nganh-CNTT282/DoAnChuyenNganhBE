using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Activities : BaseEntity
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
