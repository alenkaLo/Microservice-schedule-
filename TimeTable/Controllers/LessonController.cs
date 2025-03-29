using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Models;
using TimeTable.Data;
using TimeTable.Models.Entity;
using System.Data;

namespace TimeTable.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ApiContext _context;

        public LessonController(ApiContext context)
        {
            _context = context;
        }


        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.Lessons.ToList();

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var result = _context.Lessons.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }


        [HttpPost]
        public JsonResult Create(Lesson lesson)
        { 
            _context.Lessons.Add(lesson);

            _context.SaveChanges();
            return new JsonResult(Ok());
        }
         
    }
}
