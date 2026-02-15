using System.Net;

namespace ChalanaChithram.AuthService.Api.Middlewares;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Something went wrong"
            });
        }
    }
}
