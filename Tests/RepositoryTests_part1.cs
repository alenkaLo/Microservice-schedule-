using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TimeTable.Data;
using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

namespace RepositoryTests;

[TestClass]
public partial class LessonRepositoryTests
{
    private DbContextOptions<LessonDbContext> _options;
    private SqliteConnection _connection;
    private LessonDbContext _context;
    private LessonRepository _repository;

    [TestInitialize]
    public void Initialize()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var command = _connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = OFF;";
            command.ExecuteNonQuery();
        }

        _options = new DbContextOptionsBuilder<LessonDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new LessonDbContext(_options);
        _context.Database.EnsureCreated();

        var lessons = TestData();

        _context.Lessons.AddRange(lessons);
        _context.SaveChanges();

        _repository = new LessonRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _connection.Close();
    }


    private Lesson[] TestData()
    {
        var testLessons = new[]
        {
            GenerateLesson(new DateTime(2025, 4, 1, 13, 0, 0)),
            GenerateLesson(new DateTime(2025, 4, 1, 11, 0, 0)),
            GenerateLesson(new DateTime(2025, 4, 1, 12, 0, 0)),
            GenerateLesson(new DateTime(2025, 3, 1, 13, 0, 0)),
            GenerateLesson(new DateTime(2025, 3, 1, 11, 0, 0)),
            GenerateLesson(new DateTime(2025, 3, 1, 12, 0, 0))

        };

        return testLessons;
    }

    private Lesson GenerateLesson(DateTime? date = null)
    {
        Random rand = new Random();
        int randInt = rand.Next(10);

        DateTime lessonDate = date ?? DateTime.Now.AddHours(randInt);

        Lesson lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Subject = "",
            UserId = Guid.NewGuid(),
            ClassName = "",
            TaskID = Guid.NewGuid(),
            Date = DateOnly.FromDateTime(lessonDate),
            StartTime = TimeOnly.FromDateTime(lessonDate),
            EndTime = TimeOnly.FromDateTime(lessonDate.AddHours(1))
        };

        return lesson;
    }
}
