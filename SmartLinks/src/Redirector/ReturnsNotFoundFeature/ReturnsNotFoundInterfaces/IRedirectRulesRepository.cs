namespace Redirector;

public interface IRedirectRulesRepository
{
    Task<bool> ContainsRedirectRulesFor(string smartLink);
}
