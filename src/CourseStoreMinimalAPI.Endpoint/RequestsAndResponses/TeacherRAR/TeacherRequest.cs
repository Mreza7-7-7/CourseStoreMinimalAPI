
namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR
{
    public class TeacherRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile? File { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
