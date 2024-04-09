namespace Redirector;

public class ImplSmartLinkRedirectService(
    IRedirectableSmartLink redirectableSmartLink, 
    ISmartLinkRedirectRulesRepository repository
) : ISmartLinkRedirectService
{    
    public async Task<string?> Evaluate()
    {
        var ruleSet = await repository.Read();
        
        

        var rule = ruleSet != null ? ruleSet!.FirstOrDefault(
            r => redirectableSmartLink.IsLanguageAccepted(r.Language)
        ) : null;

        if(rule != null)
        {
          return rule.Redirect;
        }
        else
        {
            return null;
        }
    }
}
