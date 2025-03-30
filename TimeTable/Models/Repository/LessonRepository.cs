using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
