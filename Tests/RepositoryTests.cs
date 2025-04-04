using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TimeTable.Data;
using TimeTable.Models.Entity;
using TimeTable.Models.Repository;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RepositoryTests
{
    [TestClass]
    public class LessonRepositoryTests
    {
        private DbContextOptions<LessonDbContext> _options;
        private SqliteConnection _connection;
        private LessonDbContext _context;
        private LessonRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Создаем и открываем соединение SQLite in-memory
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Настраиваем DbContext с использованием SQLite
            _options = new DbContextOptionsBuilder<LessonDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new LessonDbContext(_options);
            _context.Database.EnsureCreated(); // Создаем базу данных

            _repository = new LessonRepository(_context);

            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close(); // Закрываем соединение после каждого теста
        }


        private async Task SeedTestData()
        {
            var testLessons = new[]
            {
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    SubjectId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    MarkId = Guid.NewGuid(),
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                },
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    SubjectId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    MarkId = Guid.NewGuid(),
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1)
                },
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    SubjectId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    MarkId = Guid.NewGuid(),
                    StartTime = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(3)
                }
            };

            await _context.Lessons.AddRangeAsync(testLessons);
            await _context.SaveChangesAsync();
        }


        [TestMethod]
        public async Task GetAll_ReturnsAllLessonsOrderedByStartTime()
        {
            //Arrange


            // Act
            var result = await _repository.GetAll();
            var sorted = result.OrderBy(x => x.StartTime).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(sorted, result);
        }

        [TestMethod]
        public async Task GetById_ExistingId_ReturnsLesson()
        {
            // Arrange
            var existingLesson = _context.Lessons.First();

            // Act
            var result = await _repository.GetById(existingLesson.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingLesson.Id, result.Id);
            Assert.AreEqual(existingLesson.SubjectId, result.SubjectId);
        }

        [TestMethod]
        public async Task GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _repository.GetById(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Add_ValidLesson_ReturnsIdAndAddsToDatabase()
        {
            // Arrange
            var newLesson = new Lesson
            {
                SubjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                MarkId = Guid.NewGuid(),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            // Act
            var result = await _repository.Add(newLesson);

            // Assert
            Assert.AreEqual(newLesson.Id, result);
            var lessonInDb = await _context.Lessons.FindAsync(result);
            Assert.IsNotNull(lessonInDb);
            Assert.AreEqual(newLesson.SubjectId, lessonInDb.SubjectId);
        }

        [TestMethod]
        public async Task Delete_ExistingId_DeletesLessonAndReturnsId()
        {
            // Arrange
            var lessonToDelete = _context.Lessons.First();
            var idToDelete = lessonToDelete.Id;

            // Act
            var result = await _repository.Delete(idToDelete);

            // Assert
            Assert.AreEqual(idToDelete, result);
            var deletedLesson = await _repository.GetById(idToDelete);
            Assert.IsNull(deletedLesson);
        }

        [TestMethod]
        public async Task Update_ValidData_UpdatesLessonAndReturnsId()
        {
            // Arrange
            var lessonToUpdate = _context.Lessons.First();
            var idToUpdate = lessonToUpdate.Id;
            var newSubjectId = Guid.NewGuid();
            var newUserId = Guid.NewGuid();
            var newMarkId = Guid.NewGuid();
            var newStartTime = DateTime.Now.AddDays(1);
            var newEndTime = DateTime.Now.AddDays(1).AddHours(1);

            // Act
            var result = await _repository.Update(
                idToUpdate,
                newSubjectId,
                newUserId,
                newMarkId,
                newStartTime,
                newEndTime);


            // Assert
            Assert.AreEqual(idToUpdate, result);
            var updatedLesson = await _repository.GetById(idToUpdate);
            Assert.AreEqual(newSubjectId, updatedLesson.SubjectId);
            Assert.AreEqual(newUserId, updatedLesson.UserId);
            Assert.AreEqual(newMarkId, updatedLesson.MarkId);
            Assert.AreEqual(newStartTime, updatedLesson.StartTime);
            Assert.AreEqual(newEndTime, updatedLesson.EndTime);
        }
    }
}