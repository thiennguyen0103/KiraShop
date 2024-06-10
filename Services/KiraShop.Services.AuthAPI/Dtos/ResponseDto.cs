using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KiraShop.Services.AuthAPI.Dtos
{
    public class ResponseDto<T>
    {
        public ResponseDto()
        {
        }

        public ResponseDto(T data)
        {
            Succeeded = true;
            Data = data;
        }

        public ResponseDto(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ResponseDto(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
    }
}