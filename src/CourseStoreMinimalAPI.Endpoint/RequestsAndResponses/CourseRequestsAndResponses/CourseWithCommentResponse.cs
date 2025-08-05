using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CommentRequestsAndResponses;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;

public class CourseWithCommentResponse
{
    public List<CommentResponse> Comments { get; set; }
}

