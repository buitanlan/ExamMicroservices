using System.Net;

namespace Examination.Shared.SeedWork;

public class ApiSuccessResult<T>: ApiResult<T>
{
    public ApiSuccessResult()
    {
    }
    public ApiSuccessResult(int statusCode, T resultObj) : base(statusCode, true, resultObj)
    {
    }
    public ApiSuccessResult(int statusCode, T resultObj, string message) : base(statusCode, true, resultObj, message)
    {
    }
    public ApiSuccessResult(HttpStatusCode statusCode, T resultObj) : base(statusCode, true, resultObj)
    {
    }
    public ApiSuccessResult(HttpStatusCode statusCode, T resultObj, string message) : base(statusCode, true, resultObj, message)
    {
    }
}
