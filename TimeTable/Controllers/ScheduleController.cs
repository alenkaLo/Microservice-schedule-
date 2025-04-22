using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Services;
using TimeTable.Contracts;
using TimeTable.Models.Entity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
        public async Task<ActionResult<List<LessonResponse>>> Get([FromQuery]Period period)
        {
            if (period.StartDate == DateOnly.MinValue & period.EndDate == DateOnly.MinValue)
                return BadRequest("Period is empty");

            if (period.StartDate > period.EndDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetAllForPeriod(period);
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<LessonResponse>>> GetUserSchedule(Guid id, [FromQuery] Period period)
        {
            if (period.StartDate == DateOnly.MinValue & period.EndDate == DateOnly.MinValue)
                return BadRequest("Period is empty");

            if (period.StartDate > period.EndDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetUserSchedule(id, period);
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        } 

        [HttpGet("class/{className}")]
        public async Task<ActionResult<List<LessonResponse>>> GetClassSchedule(string className, [FromQuery] Period period)
        {
            if (period.StartDate == DateOnly.MinValue & period.EndDate == DateOnly.MinValue)
                return BadRequest("Period is empty");

            if (period.StartDate > period.EndDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetClassSchedule(className, period);

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }
    }
}
