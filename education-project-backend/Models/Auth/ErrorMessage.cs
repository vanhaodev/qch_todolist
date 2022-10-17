namespace education_project_backend.Models.Auth
{
    public class ErrorMessage
    {
        public int httpStatus { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}