namespace Redirector;

public class FreezableSmartLink(ISupportedHttpRequest request, ILoadableRedirectRulesRepository repository) : IFreezableSmartLink
{
  public async Task<bool> IsFreezed()
  {
    return (await repository.Load(request.SmartLink))["state"]! == "freezed";
  }
}