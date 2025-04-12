using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Services;

namespace TimeTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public ScheduleController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(Ok( await _lessonService.GetAllLessons()));
        }

        [HttpGet("user/{id}")]
        public async Task<JsonResult> GetUserSchedule(Guid id)
        {
            return new JsonResult(Ok(await _lessonService.GetUserSchedule(id)));
        }
    }
}
