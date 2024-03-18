namespace Redirector;

using MongoDB.Driver;
using MongoDB.Bson;

public class RedirectRulesRepository(IMongoCollection<BsonDocument> collection) : IRedirectRulesRepository
{
    public async Task<bool> ContainsRedirectRulesFor(string smartLink)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("slug", smartLink);
        return  await collection.Find(filter).FirstOrDefaultAsync() != null;
    }
}
