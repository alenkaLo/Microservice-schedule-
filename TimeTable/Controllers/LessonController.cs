using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Models;
using TimeTable.Data;
using TimeTable.Models.Entity;
using System.Data;
using TimeTable.Models.Repository;
using TimeTable.Services;
using TimeTable.Contracts;

namespace TimeTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {   
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{id:guid}")]
        public async Task<JsonResult> GetById(Guid id)
        {
            var result = await _lessonService.GetLessonById(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonRessponse>>> GetAll()
        {
            var result = await _lessonService.GetAllLessons();

            if (result == null)
                return new JsonResult(NotFound());

            var response = result.Select(b => new LessonRessponse(b.Id, b.Subject, b.UserId, b.ClassName, b.TaskID, b.Date, b.StartTime, b.EndTime));

            return Ok(response);
        }


        [HttpPost]
        public JsonResult Create(Lesson lesson)
        { 
            if (_lessonService.Add(lesson).Result != Guid.Empty)
            {
                return new JsonResult(Ok());
            }
            else
            {
                return new JsonResult(BadRequest());
            }
        }
        [HttpPut("{id:guid}")]
        public async Task<JsonResult> Update(Guid id, string subject, Guid userId, string className, Guid taskId, DateTime date, DateTime startTime, DateTime endtime)
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
