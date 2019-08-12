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
    public class InstructorsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
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
        [HttpGet]
        public async Task<IActionResult> Get(string firstname, string lastname, string slackhandle)
        {
            string SqlCmdText = @"SELECT i.Id AS InstructorId, i.FirstName, i.LastName, i.SlackHandle, 
                                            i.CohortId, i.Specialty, c.CohortName 
                                        FROM Instructors i
                                        JOIN Cohorts c ON c.Id = i.CohortId";
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (firstname != null)
                    {
                        SqlCmdText = $"{SqlCmdText} WHERE i.FirstName LIKE '%' + @firstname + '%'";
                        cmd.Parameters.Add(new SqlParameter("@firstname", firstname));
                    }
                    if (lastname != null)
                    {
                        SqlCmdText = $"{SqlCmdText} WHERE i.LastName LIKE '%' + @lastname + '%'";
                        cmd.Parameters.Add(new SqlParameter("@lastname", lastname));
                    }
                    if (slackhandle != null)
                    {
                        SqlCmdText = $"{SqlCmdText} WHERE i.SlackHandle LIKE '%' + @slackhandle + '%'";
                        cmd.Parameters.Add(new SqlParameter("@slackhandle", slackhandle));
                    }

                    cmd.CommandText = SqlCmdText;

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty"))
                        };

                        instructor.Cohort = cohort;
                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return Ok(instructors);
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
