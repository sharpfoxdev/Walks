using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase {
        [HttpGet]
        public IActionResult GetAllStudents() {
            string[] studentNames = new string[] {"John", "Jane", "Jack", "Jill"};
            return Ok(studentNames);
        }
    }
}
