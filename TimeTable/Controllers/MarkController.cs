using Microsoft.AspNetCore.Mvc;
using TimeTable.Services;



namespace TimeTable.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly KafkaModule _kafkaModule;
        public MarkController(ILessonService lessonService, KafkaModule kafkaModule)
        {
            _lessonService = lessonService;
            _kafkaModule = kafkaModule;
        }
        [HttpPost]
        public async Task<JsonResult> GiveMark(DateTime Date, Guid TeacherID, Guid StudentID, string Comment, Guid LessonID, int Mark)
        {
            var lesson = await _lessonService.GetLessonById(LessonID);

            if (lesson is null)
                return new JsonResult(StatusCode(400, "No such lesson was found."));
            if (lesson.UserId != TeacherID)
                return new JsonResult(StatusCode(412, "This lesson is taught by another teacher."));
            if(Mark < 1 || Mark > 5)
                return new JsonResult(StatusCode(412, "This lesson is taught by another teacher."));

            var kafkaEvent = new
            {
                marks = new[]
                {
                    new
                    {
                        student_id = StudentID,
                        mark = Mark.ToString(), 
                        comment = Comment 
                    }
                },
                lesson_id = LessonID 
            };
            string jsonMessage = System.Text.Json.JsonSerializer.Serialize(kafkaEvent);
            return await _kafkaModule.CreateEventInKafka("mark-topic", jsonMessage);
        }
    }
}
