namespace Redirector;

using MongoDB.Driver;
using MongoDB.Bson;

public class ImplMongoDbRepository(
    IMongoCollection<BsonDocument> collection, 
    ISmartLink smartLink
) : IMongoDbRepository
{
    public async Task<BsonDocument?> Read()
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("slug", smartLink.Value);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}
