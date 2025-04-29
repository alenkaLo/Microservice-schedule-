using Microsoft.EntityFrameworkCore;

namespace RepositoryTests;

public partial class LessonRepositoryTests
{
    [TestMethod]
    public async Task GetAll_ReturnsAllLessonsOrderedByStartTime()
    {
        //Arrange


        // Act
        var result = await _repository.GetAll();
        var sorted = result.OrderBy(x => x.Date).ThenBy(x => x.StartTime).ToList();

        // Assert
        Assert.AreEqual(6, result.Count);
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
        var newLesson = GenerateLesson();

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
        // Arrange\
        var newLesson = GenerateLesson();
        newLesson.Id = _context.Lessons.First().Id;

        // Act
        var result = await _repository.Update(
            newLesson.Id,
            newLesson.Subject,
            newLesson.UserId,
            newLesson.ClassName,
            newLesson.TaskID,
            newLesson.Date,
            newLesson.StartTime,
            newLesson.EndTime);


        // Assert
        Assert.AreEqual(newLesson.Id, result);
        var updatedLesson = await _repository.GetById(newLesson.Id);
        Assert.AreEqual(newLesson.Subject, updatedLesson.Subject);
        Assert.AreEqual(newLesson.UserId, updatedLesson.UserId);
        Assert.AreEqual(newLesson.ClassName, updatedLesson.ClassName);
        Assert.AreEqual(newLesson.Date, updatedLesson.Date);
        Assert.AreEqual(newLesson.StartTime, updatedLesson.StartTime);
        Assert.AreEqual(newLesson.EndTime, updatedLesson.EndTime);
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

