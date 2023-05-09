using Classroom.WebAPI.Models.Student;

namespace Classroom.WebAPI.Models.Publication;

public class AssignmentDTO: PublicationDTO
{
    public Dictionary<int, StudentWorkDTO> StudentGrades { get; set; }
    public int MaxGrade { get; set; }
    public DateTime Deadline { get; set; }
}
