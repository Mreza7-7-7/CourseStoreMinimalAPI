namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;

public class CourseRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsOnline { get; set; }
    public IFormFile? File { get; set; }
}

