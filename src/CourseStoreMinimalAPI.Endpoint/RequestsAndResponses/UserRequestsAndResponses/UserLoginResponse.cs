namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.UserRequestsAndResponses;

public class UserLoginResponse
{
    public string JWT { get; set; }
    public DateTime ExpireTime { get; set; }
}

