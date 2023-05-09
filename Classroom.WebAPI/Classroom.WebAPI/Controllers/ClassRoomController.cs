using Classroom.WebAPI.Models.Classroom;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Classroom.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ClassRoomController: ControllerBase
{
    [HttpGet]
    [SwaggerResponse(200, "Got all classrooms successfully")]
    public async Task<ActionResult<List<ClassRoomDTO>>> GetClassrooms()
    {
        return Ok();
    }
    
    [HttpGet("{classroomId}")]
    [SwaggerResponse(200, "Entered the classroom page successfully")]
    public async Task<ActionResult<ClassRoomDTO>> GetClassroom(int classroomId)
    {
        return Ok();
    }
    //
    [HttpPost]
    [SwaggerResponse(201, "Classroom created successfully")]
    public async Task<IActionResult> PostClassroom([FromBody] CreateClassRoomDTO classroom) 
    {
        return Ok();
    }
    //
    [HttpPut("{classroomId}")]
    [SwaggerResponse(200, "Classroom updated successfully")]
    public async Task<ActionResult<ClassRoomDTO>> UpdateClassroom(int classroomId,[FromBody] UpdateClassRoomDTO classroom) //
    {
        return Ok();
    }
    
    [HttpPut("{classroomId}/join")]
    [SwaggerResponse(200, "Joined classroom successfully")]
    public async Task<ActionResult<ClassRoomDTO>> JoinClassroom(int classroomId, int userId ) //
    {
        return Ok();
    }
    
    [HttpDelete("{classroomId}")]
    [SwaggerResponse(204, "Deleted classroom with specified id")]
    [SwaggerResponse(404, "Not found such classroom with specified ids")]
    public async Task<IActionResult> DeleteClassroom(int classroomId)
    {
        return NoContent();
    }
}