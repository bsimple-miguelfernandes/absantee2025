using Application;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return result.Error?.StatusCode switch
        {
            400 => new BadRequestObjectResult(result.Error.Message),
            401 => new UnauthorizedObjectResult(result.Error.Message),
            403 => new ObjectResult(result.Error.Message) { StatusCode = 403 },
            404 => new NotFoundObjectResult(result.Error.Message),
            500 => new ObjectResult(result.Error.Message) { StatusCode = 500 },
            _ => new ObjectResult(result.Error?.Message ?? "Unknown error")
            { StatusCode = result.Error?.StatusCode ?? 500 }
        };
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return result.Error?.StatusCode switch
        {
            400 => new BadRequestObjectResult(result.Error.Message),
            401 => new UnauthorizedObjectResult(result.Error.Message),
            403 => new ObjectResult(result.Error.Message) { StatusCode = 403 },
            404 => new NotFoundObjectResult(result.Error.Message),
            500 => new ObjectResult(result.Error.Message) { StatusCode = 500 },
            _ => new ObjectResult(result.Error?.Message ?? "Unknown error")
            { StatusCode = result.Error?.StatusCode ?? 500 }
        };
    }
}
