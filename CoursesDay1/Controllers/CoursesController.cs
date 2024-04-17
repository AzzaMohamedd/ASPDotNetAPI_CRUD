using CoursesDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursesDay1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        APIContext db;
        public CoursesController(APIContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public ActionResult GetAllCourses()
        {
            List<Course> courses = db.Courses.ToList();
            if (courses.Count == 0) return NotFound(); //404 
            else return Ok(courses); //200 -> succeeded+OK
        }

        [HttpGet("{id}")]
        public ActionResult getById(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null) return NotFound(); //404 
            else return Ok(course); //200 -> succeeded+OK
        }

        [HttpGet("/api/CoursesByName/{name}")]  //change routing because it will conflict with getById
        public ActionResult couseByName(string name)
        {
            var course = db.Courses.FirstOrDefault(x => x.CrsName == name);
            if (course == null) return NotFound(); //404 
            else return Ok(course); //200 -> succeeded+OK
        }

        [HttpDelete("{id}")]
        public ActionResult deleteCourse(int id)
        {
            Course c = db.Courses.Find(id);
            if (c == null) return NotFound();  //404 
            else
            {
                db.Courses.Remove(c);
                db.SaveChanges();
                return Ok(c);   //200 -> succeeded+OK
            }
        }

        [HttpPost]
        public ActionResult AddCourse(Course c)
        {
            if (c == null) return BadRequest();  //400
            else
            {
                db.Courses.Add(c);
                db.SaveChanges();
                return CreatedAtAction("getById", new { id = c.Id }, c);  //201 -> succeeded + create (call action to display the latest added course)
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditCourse(Course c, int id)
        {
            if (c.Id != id) return BadRequest();  //400
            if (c == null) return NotFound();  //404
            db.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return NoContent();  //204 -> succeeded + no Content
        }

    }
}
