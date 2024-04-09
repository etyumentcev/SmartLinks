namespace Redirector;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class ImplRedirectRule : IRedirectRule
{
    public ImplRedirectRule()
    {
        Start = DateTime.MinValue;
        End = DateTime.MaxValue;
        Language = "any";
    }    

    public DateTime Start
    {
        get; set;
    }

    public DateTime End
    {
        get; set;
    }


    [BsonElement("language")]
    public string Language
    {
        get; set;
    }
    
    [BsonElement("redirectTo")]
    public string Redirect
    {
        get; set;
    }
}
