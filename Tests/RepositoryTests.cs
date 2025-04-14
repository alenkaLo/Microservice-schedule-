using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TimeTable.Data;
using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

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
            _connection.Close(); // Закрываем соединение после каждого теста
        }


        private Lesson[] TestData()
        {
            var testLessons = new[]
            {
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Subject = "",
                    UserId = Guid.NewGuid(),
                    ClassName = default,
                    TaskID = Guid.NewGuid(),
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    StartTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1)),
                    EndTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2))
                },
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Subject = "",
                    UserId = Guid.NewGuid(),
                    ClassName = default,
                    TaskID = Guid.NewGuid(),
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    StartTime = TimeOnly.FromDateTime(DateTime.UtcNow),
                    EndTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1))
                },
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Subject = "",
                    UserId = Guid.NewGuid(),
                    ClassName = default,
                    TaskID = Guid.NewGuid(),
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    StartTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2)),
                    EndTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(3))
                }
            };

            return testLessons;
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
            Assert.AreEqual(existingLesson.Date, result.Date);
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
                Id = Guid.NewGuid(),
                Subject = "",
                UserId = Guid.NewGuid(),
                ClassName = default,
                TaskID = Guid.NewGuid(),
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                StartTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(5)),
                EndTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(6))
            };

            // Act
            var result = await _repository.Add(newLesson);

            // Assert
            Assert.AreEqual(newLesson.Id, result);
            var lessonInDb = await _context.Lessons.FindAsync(result);
            Assert.IsNotNull(lessonInDb);
            Assert.AreEqual(newLesson.Date, lessonInDb.Date);
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
        public async Task Update_ValidAllData_UpdatesLessonAndReturnsId()
        {
            // Arrange
            var lessonToUpdate = _context.Lessons.First();
            var idToUpdate = lessonToUpdate.Id;
            string newSubject = "";
            var newUserId = Guid.NewGuid();
            var newClassName = default(string);
            var newTaskId = Guid.NewGuid();
            var newDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var newStartTime = TimeOnly.FromDateTime(DateTime.Now);
            var newEndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(1));

            // Act
            var result = await _repository.Update(
                idToUpdate,
                newSubject,
                newUserId,
                newClassName,
                newTaskId,
                newDate,
                newStartTime,
                newEndTime);


            // Assert
            Assert.AreEqual(idToUpdate, result);
            var updatedLesson = await _repository.GetById(idToUpdate);
            Assert.AreEqual(newSubject, updatedLesson.Subject);
            Assert.AreEqual(newUserId, updatedLesson.UserId);
            Assert.AreEqual(newClassName, updatedLesson.ClassName);
            Assert.AreEqual(newDate, updatedLesson.Date);
            Assert.AreEqual(newStartTime, updatedLesson.StartTime);
            Assert.AreEqual(newEndTime, updatedLesson.EndTime);
        }

        [TestMethod]
        public async Task Update_ValidPartData_UpdatesLessonAndReturnsId()
        {
            // Arrange
            var lessonToUpdate = _context.Lessons.First();
            var idToUpdate = lessonToUpdate.Id;
            var newTaskId = Guid.NewGuid();

            // Act
            var result = await _repository.Update(idToUpdate, taskId: newTaskId);


            // Assert
            Assert.AreEqual(idToUpdate, result);
            var updatedLesson = await _repository.GetById(idToUpdate);
            Assert.AreNotEqual(lessonToUpdate.TaskID, updatedLesson.TaskID);
            Assert.AreEqual(lessonToUpdate.Subject, updatedLesson.Subject);
            Assert.AreEqual(lessonToUpdate.UserId, updatedLesson.UserId);
            Assert.AreEqual(lessonToUpdate.ClassName, updatedLesson.ClassName);
            Assert.AreEqual(lessonToUpdate.StartTime, updatedLesson.StartTime);
            Assert.AreEqual(lessonToUpdate.EndTime, updatedLesson.EndTime);
        }
    }
}