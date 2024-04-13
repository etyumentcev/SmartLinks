namespace Redirector;

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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