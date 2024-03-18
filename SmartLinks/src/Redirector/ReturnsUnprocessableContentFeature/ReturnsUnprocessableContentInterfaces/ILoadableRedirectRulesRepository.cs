namespace Redirector;

using MongoDB.Bson;

public interface ILoadableRedirectRulesRepository
{
    Task<BsonDocument> Load(string SmartLink);
}