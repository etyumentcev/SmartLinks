namespace Redirector;

using System.Collections.Generic;
using MongoDB.Bson.Serialization;

public class ImplSmartLinkRedirectRulesRepository(IMongoDbRepository mongoDbRepository) : ISmartLinkRedirectRulesRepository
{
    public async Task<IEnumerable<IRedirectRule>> Read()
    {
        var doc = await mongoDbRepository.Read();
        var res = BsonSerializer.Deserialize<ImplSmartLinkRedirectRules>(doc).Rules;
        return res;
    }
}
