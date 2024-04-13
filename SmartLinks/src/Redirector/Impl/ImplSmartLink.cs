namespace Redirector;

public class ImplSmartLink(IHttpContextAccessor accessor) : ISmartLink
{
    public string Value => accessor.HttpContext!.Request.Path;
}
