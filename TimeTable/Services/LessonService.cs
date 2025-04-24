using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

namespace TimeTable.Services
{
    // Слой с бизнес логикой
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private const int week = 7;
        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
  
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await _lessonRepository.GetAll();
        }

        public async Task<Lesson> GetLessonById(Guid id)
        {
            return await _lessonRepository.GetById(id);
        }

        public async Task<Guid> Add(Lesson lesson)
        {
            if (lesson.StartTime > lesson.EndTime) 
                return Guid.Empty;
           return await _lessonRepository.Add(lesson);
        }
        public async Task<Guid[]> AddWithRepeats(Lesson lesson, List<DayOfWeek> days, DateOnly startPeriod, DateOnly endPeriod)
        {
            if (startPeriod > endPeriod)
                return Array.Empty<Guid>();
            var startTime = startPeriod;
            List<Lesson> lessonsToAdd = new();
            while (startTime <= endPeriod)
            {
                startTime = ForEach(lessonsToAdd, lesson, days, startTime, endPeriod);
            }
            return await _lessonRepository.AddList(lessonsToAdd);
        }
        private DateOnly ForEach(List<Lesson> lessons, Lesson lesson, List<DayOfWeek> days, DateOnly startTime, DateOnly endPeriod)
        {
            foreach (var day in days)
            {
                var time = Offset(startTime, day);
                if (time > endPeriod)
                    continue;
                AddWithOffset(lessons, lesson, time);
            }
            return startTime.AddDays(week);
        }
        private DateOnly Offset(DateOnly startTime, DayOfWeek day)
        {
            var time = startTime;
            var offset = (int)day - (int)time.DayOfWeek;
            time = (double)time.DayOfWeek <= (double)day
                ? time.AddDays(offset)
                : time.AddDays(week + offset);
            return time;
        }
        private void AddWithOffset(List<Lesson> lessons, Lesson lesson, DateOnly time)
        {
            lesson.Date = time;
            lesson.Id = Guid.NewGuid();
            lessons.Add(lesson.Clone());
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime)
        {
            return await _lessonRepository.Update(id, subject, userId, className, taskId, date, startTime, endtime);
        }
        public async Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetAllForPeriod(period);
        }
        public async Task<List<Lesson>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetUserLessons(id, period);
        }

        public async Task<List<Lesson>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetClassLessons(className, period);
        }
    }
}
