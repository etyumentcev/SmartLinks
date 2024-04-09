namespace Redirector;

using Microsoft.AspNetCore.Http.Headers;

public class ImplRedirectableSmartLink(IHttpContextAccessor accessor) : IRedirectableSmartLink
{
  public bool IsLanguageAccepted(string language) => 
    language == "any" || accessor.HttpContext!.Request.Headers.AcceptLanguage.Contains(language);

  public bool IsInTime(DateTime start, DateTime end)
  {
    var current = DateTime.Now;

    return start < current && current < end; 
  } 
}
