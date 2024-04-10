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

    [BsonElement("start")]
    public DateTime Start
    {
        get; set;
    }

    [BsonElement("end")]
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
