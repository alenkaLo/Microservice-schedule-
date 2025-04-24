using Microsoft.AspNetCore.Mvc;
using TimeTable.Models.Entity;
using TimeTable.Services;
using TimeTable.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;

namespace TimeTable.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {   
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LessonResponse>> GetById(Guid id)
        {
            var result = await _lessonService.GetLessonById(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonResponse>>> GetAll()
        {
            var result = await _lessonService.GetAllLessons();

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IdResponse>> Create(LessonWithOutID lessonWithoutID)
        { 
            Lesson lesson = new Lesson();
            lesson.Subject = lessonWithoutID.Subject;
            lesson.UserId = lessonWithoutID.UserId;
            lesson.ClassName = lessonWithoutID.ClassName;
            lesson.TaskID = lessonWithoutID.TaskID;
            lesson.Date = lessonWithoutID.Date;
            lesson.StartTime = lessonWithoutID.StartTime;
            lesson.EndTime = lessonWithoutID.EndTime;
            var result = await _lessonService.Add(lesson);
            if (result != Guid.Empty)
                return Ok(result);
            else
                return new JsonResult(BadRequest());
        }

        [HttpPost("CreateWithRepeats{startPeriod:DateTime}")]
        public async Task<ActionResult<IdResponse[]>> CreateWithRepeats([FromBody]LessonWithOutIDnDate lessonWithoutDate, [FromQuery] List<DayOfWeek> days, DateOnly startPeriod, DateOnly endPeriod)
        {
            Lesson lesson = new Lesson();
            lesson.Subject=lessonWithoutDate.Subject;
            lesson.UserId=lessonWithoutDate.UserId;
            lesson.ClassName=lessonWithoutDate.ClassName;
            lesson.TaskID=lessonWithoutDate.TaskID;
            lesson.StartTime=lessonWithoutDate.StartTime;
            lesson.EndTime=lessonWithoutDate.EndTime;
            var result = await _lessonService.AddWithRepeats(lesson, days, startPeriod, endPeriod);
            if (result != Array.Empty<Guid>())
                return Ok(result);
            else
                return new JsonResult(BadRequest());
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<IdResponse>> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime)
        {
            var result = await _lessonService.Update(id, subject, userId, className, taskId, date, startTime, endtime);
            if (result == Guid.Empty)
                return NotFound();

            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<IdResponse>> Delete(Guid id)
        {
            var result = await _lessonService.Delete(id);

            if (result == Guid.Empty)
                return NotFound();

            return Ok(result);
        }
    }
}
