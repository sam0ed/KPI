namespace Classroom.WebAPI.Models.Classroom;

public class ClassRoomDTO
{
    public int Id { get; set; }
    public int OwnerId {get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public string Password { get; set; } = string.Empty;
    public List<int> StudentsIds { get; set; } 
    public List<int> BlackListIds { get; set; }
    public List<int> AssignmentsIds { get; set; }
}