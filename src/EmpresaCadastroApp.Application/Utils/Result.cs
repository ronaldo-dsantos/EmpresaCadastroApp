namespace EmpresaCadastroApp.Application.Utils
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string?[] Errors { get; set; }
        public T? Data { get; set; }

        public static Result<T> Ok(T data)
        {
            return new Result<T> { Success = true, Data = data, Errors = Array.Empty<string>() };
        }

        public static Result<T> Fail(params string[] errors)
        {
            return new Result<T> { Success = false, Errors = errors };
        }
    }
}
