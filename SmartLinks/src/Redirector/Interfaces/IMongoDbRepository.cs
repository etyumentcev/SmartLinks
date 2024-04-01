namespace Redirector;

using MongoDB.Bson;

public interface IMongoDbRepository
{
    Task<BsonDocument?> Read();
}
