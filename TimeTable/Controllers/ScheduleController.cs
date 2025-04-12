using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Services;
using TimeTable.Contracts;

namespace TimeTable.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public ScheduleController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonRessponse>>> Get(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetAllForPeriod(startTime, endTime, startDate, endDate);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));

            return Ok(response);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<LessonRessponse>>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetUserSchedule(id, startTime, endTime, startDate, endDate);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));

            return Ok(response);
        } 

        [HttpGet("class/{className}")]
        public async Task<ActionResult<List<LessonRessponse>>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetClassSchedule(className, startTime, endTime, startDate, endDate);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));
            
            return Ok(response);
        }
    }
}
