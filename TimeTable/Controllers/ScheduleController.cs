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
        public async Task<ActionResult<List<LessonResponse>>> Get(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            if (startDate == DateOnly.MinValue & endDate == DateOnly.MinValue)
                return BadRequest("Date is empty");

            if (startDate > endDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetAllForPeriod(startTime, endTime, startDate, endDate);
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<LessonResponse>>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            if (startDate == DateOnly.MinValue & endDate == DateOnly.MinValue)
                return BadRequest("Date is empty");

            if (startDate > endDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetUserSchedule(id, startTime, endTime, startDate, endDate);
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        } 

        [HttpGet("class/{className}")]
        public async Task<ActionResult<List<LessonResponse>>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            if (startDate == DateOnly.MinValue & endDate == DateOnly.MinValue)
                return BadRequest("Date is empty");

            if (startDate > endDate)
                return BadRequest("Start date is greater then end date");

            var result = await _lessonService.GetClassSchedule(className, startTime, endTime, startDate, endDate);

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }
    }
}
