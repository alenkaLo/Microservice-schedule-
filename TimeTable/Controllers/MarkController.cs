using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using TimeTable.Models.Entity;
using TimeTable.Services;
using Newtonsoft.Json;
using System.IO;

namespace TimeTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public MarkController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }


        [HttpPost("{mark:int}")]
        public JsonResult Mark(int mark, Guid LessonId)
        {
            var lesson = _lessonService.GetLessonById(LessonId);

            var data = new
            {
                lesson,
                mark,
            };
            var result =_lessonService.GetLessonById(LessonId);

            return new JsonResult(Ok(data));
        }
    }
}
