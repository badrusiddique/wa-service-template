using System.Net;
using Wati.Template.Common.Enums;

namespace Wati.Template.Common.Dtos.Response
{
    public class ApiResponse<T> where T : class
    {
        /// <summary>
        /// response data
        /// </summary>
        public T Data { get; set; }

        public ErrorResponse Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// parse response object from dto input
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public static ApiResponse<T> ParseResponse(T data) =>
            IsNullOrEmpty(data)
                ? new ApiResponse<T> { StatusCode = HttpStatusCode.NotFound, Error = new ErrorResponse { Code = ErrorCode.NotFound.ToString(), Message = "the server could not find what was requested" } }
                : new ApiResponse<T> { StatusCode = HttpStatusCode.OK, Data = data };

        /// <summary>
        /// parse response object from dto input
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <returns>ApiResponse</returns>
        public static ApiResponse<T> ParseResponse(T data, HttpStatusCode code) =>
            new()
            {
                Data = data,
                StatusCode = code,
            };

        private static bool IsNullOrEmpty(T data) =>
            data == null || data is IEnumerable<object> objects && !objects.Any();
    }

    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
