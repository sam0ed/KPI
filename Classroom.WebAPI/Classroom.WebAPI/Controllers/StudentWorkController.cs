using Classroom.WebAPI.Models.Publication;
using Classroom.WebAPI.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Classroom.WebAPI.Controllers;

[ApiController]
[Route("ClassRoom/{classroomId}/Publication/Assignment/{assignmentId}/[controller]")]
public class StudentWorkController: ControllerBase
{
    //No put or post endpoints because empty attachments are present for all of the students by default
    [HttpGet]
    [SwaggerResponse(200, "Got all attachments of the given assignment successfully")]
    [SwaggerResponse(404, "Not found such assignment or classroom with specified id")]
    public async Task<ActionResult<List<StudentWorkDTO>>> GetAllStudentWork(int classroomId, int assignmentId)
    {
        return Ok();
    }
    
    [HttpGet("{studentWorkId}")]
    [SwaggerResponse(200, "Got attachment for the given assignment of the given student successfully")]
    [SwaggerResponse(404, "Not found such assignment or classroom with specified id")]
    public async Task<ActionResult<StudentWorkDTO>> GetStudentWork(int classroomId, int assignmentId, int studentId)
    {
        return Ok();
    }

    [HttpPut("{studentWorkId}")]
    [SwaggerResponse(200, "Attachment updated successfully")]
    public async Task<ActionResult<StudentWorkDTO>> UpdateStudentWork(int classroomId, int assignmentId, int studentId,
        [FromBody] StudentWorkDTO attachment)
    {
        return Ok();
    }
}