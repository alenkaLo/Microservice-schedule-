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
        public JsonResult Get()
        {
            return new JsonResult(Ok(_lessonService.GetAllLessons()));
        }

        [HttpGet("user/{id}")]
        public JsonResult GetUserSchedule(Guid id)
        {
            return new JsonResult(Ok(_lessonService.GetUserSchedule(id)));
        }
    }
}
