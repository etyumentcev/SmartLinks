namespace Redirector.Tests;

using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Mvc.Testing;
   
using MongoDB.Driver;
using MongoDB.Bson;

[Binding]
public sealed class UnprocessableContentStepDefinitions
{
  readonly WebApplicationFactory<Program> _factory;
  readonly HttpClient _client; 

  HttpResponseMessage? _response;

  readonly IMongoCollection<BsonDocument> _smartLinksCollection;

  public UnprocessableContentStepDefinitions(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = _factory.CreateClient(new WebApplicationFactoryClientOptions{ AllowAutoRedirect = false,});
    _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 

    var mongoDBSettings = _factory.Services.GetService<IConfiguration>()!.GetSection("RedirectorMongoDB")!.Get<MongoDBSettings>();
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    _smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
  }
  
  // Сценарий: Приложение отвечает 422 Unprocessable Content ошибкой на GET запрос,
  // правила редиректа умной ссылки определяют, что ее редирект временно приостановлен
  [Given("Правила редиректа умной ссылки /freezed определяют, что ее редирект временно приостановлен")]
  public async Task Given_Redirect_Rules_Define_To_Freeze_Redirect_For_freezzed_SmartLink()
  {
    await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/freezed\", \"state\": \"freezed\"}"));
  }
 
  [When("Клиент отправляет GET-запрос на url /freezed")]
  public async Task A_Client_Sends_Get_Request_On_Url_freezed()
  {
    _response = await _client!.GetAsync("/freezed");
  }
  
  [Then("Приложение отвечает 422 Unprocessable Content ошибкой")]
  public void The_App_Resonses_With_422_Unprocessable_Content_Error()
  {
    Assert.Equal(422, (int) _response!.StatusCode);
  }
  // Определение шагов завершено

  // Сценарий: Приложение не отвечает 422 Unprocessable Content ошибкой на GET запрос, 
  // если правила редиректа умной ссылки определяют, что ее редирект не был временно приостановлен
  [Given("Правила редиректа умной ссылки /unfreezed не приостанавливают временно ее редирект")]
  public async Task Given_Redirect_Rules_Define_To_Freeze_Redirect_For_unfreezzed_SmartLink()
  {
    await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/unfreezed\", \"state\": \"published\"}"));
  }
 
  [When("Клиент отправляет GET-запрос на url /unfreezed")]
  public async Task A_Client_Sends_Get_Request_On_Url_unfreezed()
  {
    _response = await _client!.GetAsync("/unfreezed");
  }
  
  [Then("Приложение не отвечает 422 Unprocessable Content ошибкой")]
  public void The_App_Resonses_With_Not_422_Unprocessable_Content_Error()
  {
    Assert.NotEqual(422, (int) _response!.StatusCode);
  }
  
  public void Dispose()
  {
    _client.Dispose();
  }
}