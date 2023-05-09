using Classroom.WebAPI.Models.Enums;

namespace Classroom.WebAPI.Models.Student;

public class StudentWorkDTO
{
    public int Id { get; set; }
    public StudentWorkStatus Status { get; set; }
    public DateTime? HandedInDateTime { get; set; }
    public int? Grade { get; set; }
    public List<IFormFile>? Attachment { get; set; }
}