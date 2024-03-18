namespace Redirector;

using MongoDB.Driver;
using MongoDB.Bson;

public class ImplRedirectRulesRepository(
    IMongoCollection<BsonDocument> collection
) : IRedirectRulesRepository, ILoadableRedirectRulesRepository
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
          var result = await collection.Find(filter).FirstOrDefaultAsync();
          document = result != null && result.GetValue("state", "published") != "deleted" ? result : null;
        }
    }
}
