using System.ComponentModel;

namespace Wati.Template.Common.Enums;

public enum ErrorCode
{
    [Description("NOT_FOUND")]
    NotFound,
    [Description("INVALID_ARG")]
    InvalidArg,
    [Description("DATABASE_FAILED")]
    DatabaseFailed,
    [Description("UNAUTHORIZED")]
    Unauthorized,
    [Description("INVALID_REQUEST_ARG")]
    InvalidRequestArg,
    [Description("RETRY_AFTER")]
    RetryAfter,
    [Description("UNKNOWN_ERROR")]
    UnknownError,
}