using Microsoft.AspNetCore.Mvc;
using TimeTable.Contracts;
using TimeTable.Models.Entity;
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

        public async Task<IdResponse> Add(Lesson lesson)
        {
            lesson.Id = Guid.NewGuid();
            try
            {
                var checkResult = await CanCreateOrUpdateLessonAsync(lesson,false);
                if (!checkResult.IsSuccess)
                {
                    return IdResponse.Failure(
                        $"Невозможно создать урок. {checkResult.ErrorMessage}");
                }

                await _lessonRepository.Add(lesson);

                return IdResponse.Success(lesson.Id);
            }
            catch (Exception ex)
            {
                return IdResponse.Failure("Ошибка при создании урока");
            }
        }
        public async Task<Guid[]> AddWithRepeats(Lesson lesson, List<DayOfWeek> filter, DateOnly startPeriod, DateOnly endPeriod)
        {
            LessonsToAdd lessonsToAdd = new LessonsToAdd(new List<Lesson>(), lesson);
            DatePeriod datePeriod = new DatePeriod(startPeriod, endPeriod, filter);
            if (!datePeriod.CheckCorrectPeriod())
                return Array.Empty<Guid>();
            datePeriod.Start(lessonsToAdd);
           return await _lessonRepository.AddList(lessonsToAdd.Lessons);
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<IdResponse> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endTime)
        {
            id = Guid.NewGuid();
            try
            {
                if(startTime != null && endTime != null ){
                var checkResult = await CanCreateOrUpdateLessonAsync(id,(TimeOnly)startTime,(TimeOnly)endTime,(DateOnly)date,className,false);
                if (!checkResult.IsSuccess)
                {
                    return IdResponse.Failure(
                        $"Невозможно обновить урок. {checkResult.ErrorMessage}");
                }
                }
                await _lessonRepository.Update(id, subject, userId, className, taskId, date, startTime, endTime);

                return IdResponse.Success(id);
            }
            catch (Exception ex)
            {
                return IdResponse.Failure("Ошибка при обновлении урока");
            }
        }
        public async Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetAllForPeriod(period);
        }
        public async Task<List<Lesson>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetUserLessons(id, period);
        }

        public async Task<List<Lesson>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var period = new Period
            {
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime
            };
            return await _lessonRepository.GetClassLessons(className, period);
        }

        public async Task<IdResponse> CanCreateOrUpdateLessonAsync(Guid id
                                                                   , TimeOnly startTime
                                                                   , TimeOnly endTime
                                                                   , DateOnly date
                                                                   ,String className
                                                                   , bool isUpdate = false)
        {
            try
            {
                Guid? excludeLessonId = isUpdate ? id : null;

                // Проверка доступности учителя
                var isTeacherAvailable = await _lessonRepository.IsTeacherAvailableAsync(
                    id,
                    date,
                    startTime,
                    endTime,
                    excludeLessonId);

                if (!isTeacherAvailable)
                {
                    return new IdResponse
                    (
                        Id: id,
                        IsSuccess: false,
                        ErrorMessage: $"Учитель уже имеет урок в период с {startTime} до {endTime} {date}"
                    );
                }

                // Проверка доступности класса
                var isClassAvailable = await _lessonRepository.IsClassAvailableAsync(
                    className,
                    date,
                    startTime,
                    endTime,
                    excludeLessonId);

                if (!isClassAvailable)
                {
                    return new IdResponse
                    (
                        Id: id,
                        IsSuccess: false,
                        ErrorMessage: $"Класс {className} уже занят в период с {startTime} до {endTime} {date}"
                    );
                }

                return new IdResponse ( Id: id, IsSuccess: true );
            }
            catch (Exception ex)
            {
                return new IdResponse
                (
                    Id: id,
                    IsSuccess: false,
                    ErrorMessage: "Внутренняя ошибка сервера при проверке доступности урока"
                );
            }
        }
    }
}
