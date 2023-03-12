using System.Net;

namespace Wati.Template.Common.Dtos.Response
{
    public class ApiResponseDto<T> where T : class
    {
        public T Data { get; set; }
        public ErrorDto Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ErrorDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
