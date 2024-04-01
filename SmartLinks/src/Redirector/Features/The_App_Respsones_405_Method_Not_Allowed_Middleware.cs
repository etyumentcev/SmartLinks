namespace Redirector;

public class The_App_Responses_405_Method_Not_Allowed_Middleware(
  ISupportedHttpRequest httpRequest
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
      if(!httpRequest.MethodIsSupported )   
      {
        httpContext.Response.StatusCode = 405;
      }
      else
      {
        await next(httpContext);  
      }
    }
}
