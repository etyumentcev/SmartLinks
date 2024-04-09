namespace Redirector;

using MongoDB.Bson.Serialization;
using System.Collections.Generic;

public class ImplSmartLinkRedirectRulesRepository(IMongoDbRepository mongoDbRepository) : ISmartLinkRedirectRulesRepository
{
  public async Task<IEnumerable<IRedirectRule>> Read()
  {
    var doc = await mongoDbRepository.Read();
    var res = BsonSerializer.Deserialize<ImplSmartLinkRedirectRules>(doc).Rules;
    return res;
  }
}
