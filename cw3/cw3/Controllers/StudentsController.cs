using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private string conString = "Data Source=db-mssql;Initial Catalog=s16767;Integrated Security=True";

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<StudentInfoDTO>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester " +
                                  "FROM Student s " +
                                  "JOIN Enrollment e ON e.IdEnrollment = s.IdEnrollment " +
                                  "JOIN Studies st ON st.IdStudy = e.IdStudy";
                con.Open();

                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new StudentInfoDTO
                    {
                        LastName = dr["LastName"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        BirthDate = dr["BirthDate"].ToString(),
                        Name = dr["Name"].ToString(),
                        Semester = dr["Semester"].ToString()
                    };
                    list.Add(st);
                }
            }
            return Ok(list);
        }

        // Zadanie 4.3 4.4 4.5
        // ');DROP TABLE Student;--
        [HttpGet("{studentIndex}")]
        public IActionResult GetStudentEnrollment(string studentIndex)
        {
            var enrollment = new Enrollment();

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT e.Semester, s.Name, e.StartDate " +
                                      "FROM Enrollment e " +
                                      "JOIN Studies s ON e.IdStudy = s.IdStudy " +
                                      "WHERE IdEnrollment = (SELECT IdEnrollment " +
                                                            "FROM Student " +
                                                            "WHERE IndexNumber = @studentIndex)";
                command.Parameters.AddWithValue("studentIndex", studentIndex);
                connection.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (!dr.Read())
                {
                    return NotFound("Nie znaleziono studenta o takim indeksie");
                }

                enrollment.Semester = dr["Semester"].ToString();
                enrollment.StudyName = dr["Name"].ToString();
                enrollment.StartDate = dr["StartDate"].ToString();
                
            }
            return Ok(enrollment);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id)
        { 
           return Ok("Aktualizacja zakonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }
    }
}