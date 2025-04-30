using Microsoft.AspNetCore.Mvc;
using TimeTable.Contracts;
using TimeTable.Models.Entity;

namespace TimeTable.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);
        Task<IdResponse> Add(Lesson lesson);
        Task<Guid[]> AddWithRepeats(Lesson lesson, List<DayOfWeek> days, DateOnly startPeriod, DateOnly endPeriod);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime);
        Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<IdResponse> CanCreateOrUpdateLessonAsync(Lesson lesson, bool isUpdate = false);
    }
}
