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
    [Route("[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public MarkController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        [HttpPost]
        public async Task<JsonResult> GiveMark(DateTime Date, Guid TeacherID, Guid StudentID, string Comment, Guid LessonID, int Mark)
        {
            var lesson = await _lessonService.GetLessonById(LessonID);

            if (lesson is null)
                return new JsonResult(NotFound("No such lesson was found."));
            if (lesson.UserId != TeacherID)
                return new JsonResult(BadRequest("This lesson is taught by another teacher."));

            var kafkaEvent = new
            {
                date = Date,
                teacherID = TeacherID,
                studentId = StudentID,
                comment = Comment,
                lessonId = LessonID,
                mark = Mark,
  
            };
            string jsonMessage = System.Text.Json.JsonSerializer.Serialize(kafkaEvent);
            return await KafkaController.CreateEventInKafka("VALERA-LOX", jsonMessage);
        }
    }
}
