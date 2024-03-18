namespace Redirector;

public class The_App_Responses_404_Not_Found_Middleware(
  IRedirectRulesRepository rulesSet, 
  ISupportedHttpRequest httpRequest
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        if(!await rulesSet.ContainsRedirectRulesFor(httpRequest.SmartLink) && httpRequest.MethodIsSupported)
        {
          httpContext.Response.StatusCode = 404;
        }
        else
        {
          await next(httpContext);  
        }
    }
}
