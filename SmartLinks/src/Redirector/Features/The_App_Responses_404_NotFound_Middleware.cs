namespace Redirector;

public class The_App_Responses_404_Not_Found_Middleware(
  IStatableSmartLinkRepository repository
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
      var rules = await repository.Read();
      if(rules == null || rules!.State == "deleted")   
      {
        httpContext.Response.StatusCode = 404;
      }
      else
      {
        await next(httpContext);  
      }
    }
}
