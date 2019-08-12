using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using StudentExercisesAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace StudentExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET api/values
       /* [HttpGet]
        public async Task<IActionResult> Get(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())

                    {
                        cmd.CommandText = @"SELECT s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, 
                                            s.CohortId, c.CohortName 
                                        FROM Students s
                                        JOIN Cohorts c ON c.Id = s.CohortId";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = cohort
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return Ok(students);
                }
            }
        } */
        
    // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            string SqlCmdText = @"SELECT s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, 
                                            s.CohortId, c.CohortName 
                                        FROM Students s
                                        JOIN Cohorts c ON c.Id = s.CohortId";

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())

                    {
                    cmd.CommandText = SqlCmdText;

                    if (query != null)
                    {
                        SqlCmdText = $"{SqlCmdText} WHERE s.FirstName LIKE '%' + @query + '%'";
                        cmd.Parameters.Add(new SqlParameter("@query", query));
                    }

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = cohort
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return Ok(students);
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
