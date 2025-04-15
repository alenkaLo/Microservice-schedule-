﻿using Microsoft.AspNetCore.Mvc;
using TimeTable.Models.Entity;
using TimeTable.Services;
using TimeTable.Contracts;

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
        public ActionResult Create(Lesson lesson)
        { 
            var result = _lessonService.Add(lesson);
            if (result.Result != Guid.Empty)
            {
                return Ok(result);
            }
            else
            {
                return new JsonResult(BadRequest());
            }
        }
        [HttpPost("CreateWithRepeat")]
        public JsonResult CreateWithRepeats([FromBody]Lesson lesson, [FromQuery]List<DateTime> days, DateOnly startPeriod, DateOnly endPeriod)
        {
            _lessonService.AddWithRepeats(lesson, days, startPeriod, endPeriod);
            return new JsonResult(Ok());
        }
        [HttpPut("{id:guid}")]
        public async Task<JsonResult> Update(Guid id, string subject, Guid userId, string className, Guid taskId, DateOnly date, TimeOnly startTime, TimeOnly endtime)
        {
            var result = await _lessonService.Update(id, subject, userId, className, taskId, date, startTime, endtime);
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<JsonResult> Delete(Guid id)
        {
            var result = await _lessonService.Delete(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }
    }
}
