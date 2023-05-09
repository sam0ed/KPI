using Classroom.WebAPI.Models.Enums;

namespace Classroom.WebAPI.Models.Publication;

public class PublicationDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublishingDate { get; set; }
    public PublicationType Type { get; set; }
}