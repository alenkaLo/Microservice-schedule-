using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using TimeTable.Data;
using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    //Слой для взаимодействия с бд
    public class LessonRepository:ILessonRepository
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
                .OrderBy(l => l.StartTime)
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
            await _dbContext.Lessons.AddAsync(lesson);
            await _dbContext.SaveChangesAsync();
            return lesson.Id;   
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Lessons
                 .Where(x => x.Id == id)
                 .ExecuteDeleteAsync();
            return id;

        }

        public async Task<Guid> Update(Guid id, Guid subjectId, Guid userId, Guid markId, DateTime startTime, DateTime endtime)
        {
            await _dbContext.Lessons
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.SubjectId, x => subjectId)
                .SetProperty(x => x.UserId, x => userId)
                .SetProperty(x => x.MarkId, x => markId)
                .SetProperty(x => x.StartTime, x => startTime)
                .SetProperty(x => x.EndTime, x => endtime));
            return id;
        }
    }
}
