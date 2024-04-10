namespace Redirector;

public interface ISmartLinkRedirectService
{
    Task<string?> Evaluate();
}
