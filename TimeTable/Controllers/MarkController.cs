using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using TimeTable.Models.Entity;
using TimeTable.Services;
using Newtonsoft.Json;
using System.IO;
using Confluent.Kafka;
using System.Text.Json;


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
        [HttpPost]
        public async Task<JsonResult> GiveMark(/*Guid TeacherID, Guid StudentID, Guid LessonID, int Mark*/)
        {
            //var lesson = await _lessonService.GetLessonById(LessonID);
            //if (lesson is null)
            //    return new JsonResult(NotFound("No such lesson was found."));

            //if (lesson.UserId != TeacherID)
            //    return new JsonResult(BadRequest("This lesson is taught by another teacher."));

            // Создание события
            var kafkaEvent = new
            {
                //StudentId = StudentID,
                //LessonId = LessonID,
                //Mark = Mark,
                Timestamp = DateTime.UtcNow
            };
            string jsonMessage = System.Text.Json.JsonSerializer.Serialize(kafkaEvent);
            return await KafkaController.CreateEventInKafka("VALERA-LOX", "xyu");
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
            var result = _lessonService.GetLessonById(LessonId);

            return new JsonResult(Ok(data));
        }
    }
}
