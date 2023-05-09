namespace Classroom.WebAPI.Models.Student;

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<int> ClassesIds { get; set; }
}