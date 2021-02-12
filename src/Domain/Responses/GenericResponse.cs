namespace Domain.Responses
{
    public class GenericResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ResponseCode { get; set; } = 0;
    }
}