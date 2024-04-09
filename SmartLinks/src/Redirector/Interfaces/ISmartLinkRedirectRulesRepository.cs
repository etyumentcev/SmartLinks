namespace Redirector;

using System.Collections.Generic;

public interface ISmartLinkRedirectRulesRepository
{
    Task<IEnumerable<IRedirectRule>> Read();
}