namespace Redirector;

public class ImplSupportedHttpRequest(IHttpContextAccessor accessor) : ISupportedHttpRequest
{
    static readonly string[] supportedHttpMethods = new string[] { "GET", "HEAD" };

    public bool MethodIsSupported => supportedHttpMethods.Contains(accessor.HttpContext!.Request.Method);
}
