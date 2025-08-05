namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CommentRequestsAndResponses
{
    public class CommentRequest
    {
        public string CommentBody { get; set; }
        public DateTime CommentDate { get; set; }
        public int CourseId { get; set; }
    }
}
