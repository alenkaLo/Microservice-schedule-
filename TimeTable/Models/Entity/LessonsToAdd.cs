using System;

namespace TimeTable.Models.Entity
{
    public class LessonsToAdd
    {
        public List<Lesson> Lessons { get; set; }
        public Lesson  UniqueLesson { get; set; }
        public LessonsToAdd(List<Lesson> lessons, Lesson uniqueLesson)
        {
            Lessons = lessons;
            UniqueLesson = uniqueLesson;
        }
        public void Add(DateOnly time)
        {
            UniqueLesson.Date = time;
            UniqueLesson.Id = Guid.NewGuid();
            Lessons.Add(UniqueLesson.Clone());
        }
    }
}
