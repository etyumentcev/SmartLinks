namespace Redirector;

public interface ISupportedHttpRequest
{
  bool MethodIsSupported
  {
    get;
  }
  string SmartLink
  {
    get;
  }
}
