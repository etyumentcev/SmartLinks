namespace Redirector;

public class ImplFreezeSmartLinkService(
  IStatableSmartLinkRepository repository
) : IFreezeSmartLinkService
{
    public async Task<bool> ShouldSmartLinkBeFreezed()
    {
        return (await repository.Read())!.State == "freezed";
    }
}
