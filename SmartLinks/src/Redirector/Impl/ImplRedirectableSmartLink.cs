namespace Redirector;

using Microsoft.AspNetCore.Http.Headers;

public class ImplRedirectableSmartLink(IHttpContextAccessor accessor) : IRedirectableSmartLink
{
    public bool IsLanguageAccepted(string language)
    {
        var acceptLanguage = accessor.HttpContext!.Request.Headers.AcceptLanguage.ToString();
        return language == "any" || acceptLanguage.Contains('*') || acceptLanguage.Contains(language, StringComparison.InvariantCultureIgnoreCase);
    }
    public bool IsInTime(DateTime start, DateTime end)
    {
        var current = DateTime.Now;

        return start < current && current < end;
    }
}
