namespace ShipConnect.Helpers
{
    public class GeneralResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation completed successfully";
        public T? Data { get; set; }

        public static GeneralResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new GeneralResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static GeneralResponse<T> FailResponse(string message)
        {
            return new GeneralResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
