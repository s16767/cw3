using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DTOs;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        private EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            Enrollment enrollment = _service.EnrollStudent(request);
            if(enrollment == null)
            {
                return BadRequest("Blad");
            }
            else
            {
                return Ok(enrollment);
            }
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {

           string msg = _service.PromoteStudent(request);
            if(msg.Equals("Zaktualizowano wpisy"))
            {
                return Ok(msg);
            }
            else
            {
                return BadRequest(msg);
            }
        }
    }
}