namespace Redirector.Tests;

using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Mvc.Testing;
   
using MongoDB.Driver;
using MongoDB.Bson;

[Binding]
public sealed class TemporaryRediectStepDefinitions
{
  readonly WebApplicationFactory<Program> _factory;
  readonly HttpClient _client; 

  HttpResponseMessage? _response;

    readonly IMongoCollection<BsonDocument> _smartLinksCollection;

  public TemporaryRediectStepDefinitions(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = _factory.CreateClient(new WebApplicationFactoryClientOptions{ AllowAutoRedirect = false,});
    _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 

    var mongoDBSettings = _factory.Services.GetService<IConfiguration>()!.GetSection("RedirectorMongoDB")!.Get<MongoDBSettings>();
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    _smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
  }
  
  // Функция: риложение отвечает 307 Temporary Redirect на GET запрос, 
  // если для запрашиваемой умной ссылки определено одно безусловное правило редиректа

  [Given("Для умной ссылки /unconditional-redirect определено правило редиректа")]
  public async void Given_One_Redirect_Rule_Is_Defined_For_unconidtional_redirect_SmartLink()
  {
    await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/unconditional-redirect\", rules: [{redirectTo: \"hwdtech.ru\"}]}"));
  }
 
  [When("Клиент отправляет GET-запрос на url /unconditional-redirect")]
  public async Task A_Client_Sends_Get_Request_On_Url_uncoditional_redirect()
  {
    _response = await _client!.GetAsync("/unconditional-redirect");
  }
  
  [Then("Приложение отвечает 307 Temporary Rediect")]
  public void The_App_Resonses_With_307_Temporary_Redirect_Error()
  {
    Assert.Equal(307, (int) _response!.StatusCode);
  }

  // Определение шагов завершено

  
}