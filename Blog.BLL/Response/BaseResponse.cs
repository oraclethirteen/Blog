namespace Blog.BLL.Response
{
    public class BaseResponse
    {
        public bool Success { get; protected set; }
        public string Message { get; set; }
        public List<string> ModelErrors { get; protected set; }

        public BaseResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}