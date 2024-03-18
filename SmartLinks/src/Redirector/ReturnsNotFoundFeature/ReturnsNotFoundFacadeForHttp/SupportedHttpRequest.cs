namespace Redirector;

public class SupportedHttpRequest(IHttpContextAccessor accessor) : ISupportedHttpRequest
{
  static string[] supportedHttpMethods = new string[] {"GET", "HEAD"};

  public bool MethodIsSupported => supportedHttpMethods.Contains(accessor.HttpContext!.Request.Method);

  public string SmartLink => accessor.HttpContext!.Request.Path;
}