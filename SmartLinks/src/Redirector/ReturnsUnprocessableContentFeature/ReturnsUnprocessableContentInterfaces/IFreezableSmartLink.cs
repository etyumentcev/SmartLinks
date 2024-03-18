namespace Redirector;

public interface IFreezableSmartLink
{
  Task<bool> IsFreezed();
}
