using System;

namespace TimeTable.Models.Entity
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid UserId { get; set; }
        public Guid MarkID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Subgect? Subgect { get; set; }
    }
}
