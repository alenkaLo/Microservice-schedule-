﻿using TimeTable.Models.Entity;
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
        public async Task<Guid> AddWithRepeat(Lesson lesson, List<DateTime> days, DateTime startPeriod, DateTime endPeriod)
        {
            var startTime = startPeriod.Date;
            if (startTime > endPeriod)
            {
                var time = startTime;
                startTime = endPeriod;
                endPeriod = time;
            }
            List<Lesson> lessonsToAdd = new();
            while(startTime < endPeriod)
            {
                foreach(var day in days)
                {
                    var time = startTime;
                    if ((double)time.DayOfWeek < (double)day.DayOfWeek)
                        time = time.AddDays((double)day.DayOfWeek - (double)time.DayOfWeek);
                    else
                        time = time.AddDays(week - ((double)time.DayOfWeek - (double)day.DayOfWeek));
                    if (time > endPeriod) 
                        break;
                    Lesson currentLesson = lesson.Clone();
                    currentLesson.Id = Guid.NewGuid();
                    currentLesson.Date = time;
                    lessonsToAdd.Add(currentLesson);
                }
                startTime = startTime.AddDays(week);
            }
            await _lessonRepository.AddList(lessonsToAdd);
            return lesson.Id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, string subject, Guid userId, string className, Guid taskId, DateTime date, TimeOnly startTime, TimeOnly endtime)
        {
            return await _lessonRepository.Update(id, subject, userId, className, taskId, date, startTime, endtime);
        }

        public async Task<List<Lesson>> GetUserSchedule(Guid id)
        {
            return await _lessonRepository.GetUserLessons(id);
        }
    }
}
