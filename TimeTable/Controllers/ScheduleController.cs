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
        public async Task<ActionResult<List<LessonResponse>>> Get(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetAllForPeriod(startTime, endTime, startDate, endDate);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<LessonResponse>>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetUserSchedule(id, startTime, endTime, startDate, endDate);
            if (result is null)
                return NotFound();
    
            return Ok(result);
        } 

        [HttpGet("class/{className}")]
        public async Task<ActionResult<List<LessonResponse>>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            var result = await _lessonService.GetClassSchedule(className, startTime, endTime, startDate, endDate);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }
    }
}
