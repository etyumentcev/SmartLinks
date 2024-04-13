namespace Redirector;

public class The_App_Responses_307_Temporary_Redirect_Middleware(
  ISmartLinkRedirectService smartLinkRedirectService
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        var redirect = await smartLinkRedirectService.Evaluate();

        if (redirect != null)
        {
            httpContext.Response.StatusCode = 307;
            httpContext.Response.Headers.Location = redirect;
        }
        else
        {
            await next(httpContext);
        }
    }
}