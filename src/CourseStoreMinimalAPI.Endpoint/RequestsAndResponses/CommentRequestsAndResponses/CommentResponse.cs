namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CommentRequestsAndResponses
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string CommentBody { get; set; }
        public DateTime CommentDate { get; set; }
        public int CourseId { get; set; }
    }
}
