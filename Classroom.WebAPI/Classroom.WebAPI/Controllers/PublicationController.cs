using Classroom.WebAPI.Models.Publication;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Classroom.WebAPI.Controllers;

[ApiController]
[Route("ClassRoom/{classroomId}/[controller]")]
public class PublicationController:ControllerBase
{

    [HttpGet]
    [SwaggerResponse(200, "Got all assignments of the given classroom successfully")]
    [SwaggerResponse(404, "Not found such classroom with specified id")]
    public async Task<ActionResult<List<MaterialDTO>>> GetPublications(int classroomId)
    {
        return Ok();
    }
    
    [HttpGet("Assignment/{assignmentId}")]
    [SwaggerResponse(200, "Entered the publication page successfully")]
    [SwaggerResponse(404, "Not found such classroom or assignment with specified id")]
    public async Task<ActionResult<AssignmentDTO>> GetAssignment(int classroomId, int assignmentId)
    {
        return Ok();
    }
    
    [HttpGet("Material/{materialId}")]
    [SwaggerResponse(200, "Entered the publication page successfully")]
    [SwaggerResponse(404, "Not found such classroom or material with specified id")]
    public async Task<ActionResult<MaterialDTO>> GetMaterial(int classroomId, int materialId)
    {
        return Ok();
    }
    
    [HttpPost("Assignment")]
    [SwaggerResponse(201, "Assignment was created successfully")]
    public async Task<IActionResult> PostAssignment(int classroomId,[FromBody] AssignmentDTO assignment)
    {
        return Ok();
    }
    
    [HttpPost("Material")]
    [SwaggerResponse(201, "Material was created successfully")]
    public async Task<IActionResult> PostMaterial(int classroomId,[FromBody] MaterialDTO material)
    {
        return Ok();
    }
    
    [HttpPut("Assignment/{assignmentId}")]
    [SwaggerResponse(200, "Assignment updated successfully")]
    public async Task<ActionResult<AssignmentDTO>> UpdateAssignment(int classroomId,int assignmentId,[FromBody] AssignmentDTO assignment) //
    {
        return Ok();
    }
    
    [HttpPut("Material/{materialId}")]
    [SwaggerResponse(200, "Material updated successfully")]
    public async Task<ActionResult<MaterialDTO>> UpdateMaterial(int classroomId,int materialId,[FromBody] MaterialDTO material) //
    {
        return Ok();
    }
    
    [HttpDelete]
    [SwaggerResponse(204, "Deleted publication with specified id")]
    [SwaggerResponse(404, "Not found such publication with specified ids")]
    public async Task<IActionResult> DeletePublication(int publicationId)
    {
        return NoContent();
    }
}