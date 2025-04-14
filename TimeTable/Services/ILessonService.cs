﻿using System;
using TimeTable.Models.Entity;

namespace TimeTable.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);
        Task<Guid> Add(Lesson lesson);
        Task<Guid> AddWithRepeat(Lesson lesson, List<DateTime> days, DateTime startPeriod, DateTime endPeriod);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, String subject, Guid userId, string className, Guid taskId, DateOnly date, TimeOnly startTime, TimeOnly endtime);
        Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
    }
}
