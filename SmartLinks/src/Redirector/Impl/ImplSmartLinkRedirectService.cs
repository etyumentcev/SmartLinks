namespace Redirector;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

public interface IRedirectRule
{
    string Redirect
    {
        get; set;
    }
}

[BsonIgnoreExtraElements]
public class ImplRedirectRule : IRedirectRule
{
    [BsonElement("redirectTo")]
    public string Redirect
    {
        get; set;
    }
}

public interface ISmartLinkRedirectRulesRepository
{
    Task<IEnumerable<IRedirectRule>> Read();
}

[BsonIgnoreExtraElements]
public class ImplSmartLinkRedirectRules
{
  [BsonElement("rules")]
  public List<ImplRedirectRule> Rules
  {
    get;
    set; 
  }
}


public class ImplSmartLinkRedirectRulesRepository(IMongoDbRepository mongoDbRepository) : ISmartLinkRedirectRulesRepository
{
  public async Task<IEnumerable<IRedirectRule>> Read()
  {
    var doc = await mongoDbRepository.Read();
    var res = BsonSerializer.Deserialize<ImplSmartLinkRedirectRules>(doc).Rules;
    return res;
  }
}

public class ImplSmartLinkRedirectService(ISmartLinkRedirectRulesRepository repository) : ISmartLinkRedirectService
{    
    public async Task<string?> Evaluate()
    {
        var ruleSet = await repository.Read();
        
        var rule = ruleSet != null ? ruleSet!.FirstOrDefault(r => true) : null;

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
