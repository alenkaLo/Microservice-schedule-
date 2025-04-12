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
        public async Task<ActionResult<List<LessonRessponse>>> Get(string startTime, string endTime, string startDate, string endDate)
        {
            TimeOnly.TryParse(startTime, out var startTime2);
            TimeOnly.TryParse(endTime, out var endTime2);
            DateOnly.TryParse(startDate, out var startDate2);   
            DateOnly.TryParse(endDate, out var endDate2);
            var result = await _lessonService.GetAllLessons(startTime2, endTime2, startDate2, endDate2);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));

            return Ok(response);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<LessonRessponse>>> GetUserSchedule(Guid id, string startTime, string endTime, string startDate, string endDate)
        {
            TimeOnly.TryParse(startTime, out var startTime2);
            TimeOnly.TryParse(endTime, out var endTime2);
            DateOnly.TryParse(startDate, out var startDate2);   
            DateOnly.TryParse(endDate, out var endDate2);
            var result = await _lessonService.GetUserSchedule(id, startTime2, endTime2, startDate2, endDate2);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));

            return Ok(response);
        } 

        [HttpGet("class/{className}")]
        public async Task<ActionResult<List<LessonRessponse>>> GetClassSchedule(string className)
        {
            var result = await _lessonService.GetClassSchedule(className);
            if (result == null)
                return new JsonResult(NotFound());
    
            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date.ToString(), b.StartTime.ToString(), b.EndTime.ToString()));
            
            return Ok(response);
        }
    }
}
