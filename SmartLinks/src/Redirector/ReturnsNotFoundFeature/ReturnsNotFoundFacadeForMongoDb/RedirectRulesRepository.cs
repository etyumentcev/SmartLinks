namespace Redirector;

using MongoDB.Driver;
using MongoDB.Bson;

public class RedirectRulesRepository(IMongoCollection<BsonDocument> collection) 
  : IRedirectRulesRepository,
    ILoadableRedirectRulesRepository
{
    BsonDocument? document = null;

    public async Task<bool> ContainsRedirectRulesFor(string smartLink)
    {
        await TryToLoad(smartLink);
        return document != null;
    }
    
    public async Task<BsonDocument> Load(string smartLink)
    {
        await TryToLoad(smartLink);
        
        return document!;
    }

    private async Task TryToLoad(string smartLink)
    {
        if(document == null)
        {
          FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("slug", smartLink);
          document = await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
