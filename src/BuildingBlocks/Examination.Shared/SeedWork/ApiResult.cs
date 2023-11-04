using System.Net;

namespace Examination.Shared.SeedWork;

public class ApiResult<T>
{
    public bool IsSucceed { get; set; }
    public string Message { get; set; }
    public T ResultObj { get; set; }
    public int StatusCode { set; get; }
    public ApiResult()
    {
    }
    public ApiResult(HttpStatusCode statusCode, bool isSucceed , string message = null)
    {
        Message = message;
        IsSucceed = isSucceed ;
        StatusCode = (int)statusCode;
    }

    public ApiResult(int statusCode, bool isSucceed , string message = null)
    {
        Message = message;
        IsSucceed = isSucceed ;
        StatusCode = statusCode;
    }

    public ApiResult(HttpStatusCode statusCode, bool isSucceed, T resultObj, string message = null)
    {
        ResultObj = resultObj;
        Message = message;
        IsSucceed = isSucceed;
        StatusCode = (int)statusCode;
    }

    public ApiResult(int statusCode, bool isSucceed, T resultObj, string message = null)
    {
        ResultObj = resultObj;
        Message = message;
        IsSucceed = isSucceed;
        StatusCode = statusCode;
    }
}
