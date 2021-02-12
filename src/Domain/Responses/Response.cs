namespace Domain.Responses
{
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ResponseCode { get; set; } = 0;
    }
}