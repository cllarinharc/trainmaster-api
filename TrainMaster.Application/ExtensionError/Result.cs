using FluentValidation.Results;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.ExtensionError
{
    public class Result<T>
    {
        public Result(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public Result(bool success, IEnumerable<ValidationFailure> errors)
        {
            Success = success;
            Errors = errors;
        }

        public Result() { }

        public bool Success { get; set; }
        public T Data { get; set; }
        public string? AccessToken { get; set; }
        public string Message { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; }

        public static Result<T> Ok(string responseMessage = null, T responseData = default)
        {
            return new Result<T>(success: true, message: responseMessage, data: responseData);
        }

        public static Result<T> Error(string responseMessage)
        {
            return new Result<T>(success: false, responseMessage, data: default);
        }

        public static Result<T> OkLogin(T responseData)
        {
            return new Result<T>(success: true, message: "Login realizado com sucesso", data: responseData);
        }

        public static Result<T> OkWithData(T responseData)
        {
            return new Result<T>(success: true, message: string.Empty, data: responseData);
        }

        public static Result<T> Okedit(T data)
        {
            return new Result<T> { Success = true, Data = data };
        }

        public static Result<DepartmentEntity> OkDepartment(DepartmentEntity data)
        {
            return new Result<DepartmentEntity>
            {
                Success = true,
                Message = "Departamento carregado com sucesso",
                Data = data
            };
        }
    }
}