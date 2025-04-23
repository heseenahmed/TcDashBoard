namespace TCDashBoard.Dtos
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string? Error { get; set; }
        public T? Result { get; set; }

        public static ApiResponse<T> Success(T result) =>
            new ApiResponse<T> { Status = true, Result = result };

        public static ApiResponse<T> Fail(string error) =>
            new ApiResponse<T> { Status = false, Error = error };
    }
}
