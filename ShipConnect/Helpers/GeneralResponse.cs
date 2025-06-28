namespace ShipConnect.Helpers
{
    public class GeneralResponse<T>
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = "Operation completed successfully";

        public T? Data { get; set; }

        public static GeneralResponse<T> SuccessResponse(string message = "Success", T? data=default)
        {
            return new GeneralResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static GeneralResponse<T> FailResponse(string message, T? data = default)
        {
            return new GeneralResponse<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
}
