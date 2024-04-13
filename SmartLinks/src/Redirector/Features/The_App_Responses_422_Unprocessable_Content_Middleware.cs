namespace Redirector;

public class The_App_Responses_422_Uprocessable_Content_Middleware(
  IFreezeSmartLinkService freezableSmartLinkService
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        if (await freezableSmartLinkService.ShouldSmartLinkBeFreezed())
        {
            httpContext.Response.StatusCode = 422;
        }
        else
        {
            await next(httpContext);
        }
    }
}