namespace Redirector;

using MongoDB.Bson.Serialization;

public class ImplStatableSmartLinkRepository(IMongoDbRepository mongoDbRepository) : IStatableSmartLinkRepository
{
  public async Task<IStatableSmartLink?> Read()
  {
    var doc = await mongoDbRepository.Read();
    return doc != null ? BsonSerializer.Deserialize<ImplStatableSmartLink>(doc) : null;
  }
}