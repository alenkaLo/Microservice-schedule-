using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Logging;
using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    //Слой для взаимодействия с бд
    public class LessonRepository : ILessonRepository
    {
        private readonly LessonDbContext _dbContext;
        public LessonRepository(LessonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Lesson>> GetAll()
        {
            return await _dbContext.Lessons
                .AsNoTracking()
                .OrderBy(l => l.Date)
                .ThenBy(l => l.StartTime)
                .ToListAsync();
        }

        public async Task<Lesson> GetById(Guid id)
        {
            return await _dbContext.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Guid> Add(Lesson lesson)
        {
            try
            {
                if (lesson.Id == Guid.Empty) lesson.Id = Guid.NewGuid();
                await _dbContext.Lessons.AddAsync(lesson);
                _dbContext.SaveChanges();//TODO Nikita await _dbContext.SaveChangesAsync() 
                return lesson.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }
        public async Task<Guid[]> AddList(List<Lesson> lessons)
        {
            if (lessons == null || !lessons.Any())
            {
                return Array.Empty<Guid>();
            }
            await _dbContext.Lessons.AddRangeAsync(lessons);
            _dbContext.SaveChanges();
            List<Guid> indexes = new List<Guid>();
            foreach (var lesson in lessons)
            {
                indexes.Add(lesson.Id);
            }
            return indexes.ToArray();
        }

        public async Task<Guid> Delete(Guid id)
        {
            var query = _dbContext.Lessons.Where(x => x.Id == id);

            if (!query.Any())
            {
                ConsoleLogger.Logger.LogInformation($"Объект {id} для удаления не найден");
                return Guid.Empty;
            }

            await query.ExecuteDeleteAsync();
            return id;
        }
        public async Task<Guid> Update(
            Guid id,
            string? subject = null,
            Guid? userId = null,
            string? className = null,
            Guid? taskId = null,
            DateOnly? date = null,
            TimeOnly? startTime = null,
            TimeOnly? endTime = null)
        {
            var query = _dbContext.Lessons.Where(x => x.Id == id);

            if (!query.Any())
            {
                ConsoleLogger.Logger.LogInformation($"Объект {id} для обновления не найден");
                return Guid.Empty;
            }

            await query.ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Subject, x => subject ?? x.Subject)
                .SetProperty(x => x.UserId, x => userId ?? x.UserId)
                .SetProperty(x => x.ClassName, x => className ?? x.ClassName)
                .SetProperty(x => x.TaskID, x => taskId ?? x.TaskID)
                .SetProperty(x => x.Date, x => date ?? x.Date)
                .SetProperty(x => x.StartTime, x => startTime ?? x.StartTime)
                .SetProperty(x => x.EndTime, x => endTime ?? x.EndTime));

            return id;
        }
        public async Task<List<Lesson>> GetAllForPeriod(Period period)
        {
            return await _dbContext.Lessons
            .Where(l => l.Date > period.StartDate ||
                   (l.Date == period.StartDate && l.StartTime >= period.StartTime))
            .Where(l => l.Date < period.EndDate ||
                   (l.Date == period.EndDate && l.StartTime <= period.EndTime))
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<Lesson>> GetUserLessons(Guid userid, Period period)
        {
            return await _dbContext.Lessons
            .Where(x => x.UserId == userid)
            .Where(l => l.Date > period.StartDate ||
                   (l.Date == period.StartDate && l.StartTime >= period.StartTime))
            .Where(l => l.Date < period.EndDate ||
                   (l.Date == period.EndDate && l.StartTime <= period.EndTime))
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<Lesson>> GetClassLessons(string className, Period period)
        {
            return await _dbContext.Lessons
            .Where(x => x.ClassName == className)
            .Where(l => l.Date > period.StartDate ||
                   (l.Date == period.StartDate && l.StartTime >= period.StartTime))
            .Where(l => l.Date < period.EndDate ||
                   (l.Date == period.EndDate && l.StartTime <= period.EndTime))
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .AsNoTracking()
            .ToListAsync();
        }

        //Метод проверки доступности учителя
        public async Task<bool> IsTeacherAvailableAsync(Guid teacherId, DateOnly date, TimeOnly startTime, TimeOnly endTime, Guid? excludeLessonId = null)
        {
            return await _dbContext.Lessons
                .Where(l => l.UserId == teacherId && l.Date == date)
                .Where(l => excludeLessonId == null || l.Id != excludeLessonId)
                .AllAsync(l => l.EndTime < startTime || l.StartTime > endTime);
        }

        //Метод проверки доступности урока
        public async Task<bool> IsClassAvailableAsync(string className, DateOnly date, TimeOnly startTime, TimeOnly endTime, Guid? excludeLessonId = null)
        {
            return await _dbContext.Lessons
                .Where(l => l.ClassName == className && l.Date == date)
                .Where(l => excludeLessonId == null || l.Id != excludeLessonId)
                .AllAsync(l => l.EndTime < startTime || l.StartTime > endTime);
        }
    }
}
