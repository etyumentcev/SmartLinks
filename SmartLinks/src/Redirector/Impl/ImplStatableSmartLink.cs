namespace Redirector;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class ImplStatableSmartLink : IStatableSmartLink
{
  [BsonElement("state")]
  [BsonDefaultValue("undefined")]
  public string State
  {
    get;
    set;
  }
}
