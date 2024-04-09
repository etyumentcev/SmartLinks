namespace Redirector;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

[BsonIgnoreExtraElements]
public class ImplSmartLinkRedirectRules
{
  [BsonElement("rules")]
  public List<ImplRedirectRule> Rules
  {
    get;
    set; 
  }
}