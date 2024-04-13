namespace Redirector;

public interface IFreezeSmartLinkService
{
    Task<bool> ShouldSmartLinkBeFreezed();
}
