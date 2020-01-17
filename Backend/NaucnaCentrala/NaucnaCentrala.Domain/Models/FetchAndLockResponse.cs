namespace NaucnaCentrala.Domain.Models
{
    public class FetchAndLockResponse
    {
        public FetchAndLockResponse(bool success, FetchAndLockResponseEntry data = null)
        {
            Success = success;
            ResponseData = data;
        }

        public bool Success { get; set; }
        public FetchAndLockResponseEntry ResponseData { get; set; }
    }
}
