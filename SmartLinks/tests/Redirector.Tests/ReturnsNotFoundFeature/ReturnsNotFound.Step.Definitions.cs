namespace Redirector.Tests;

using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Mvc.Testing;
   
using MongoDB.Driver;
using MongoDB.Bson;

[Binding]
public sealed class NotFoundStepDefinitions
{
  readonly WebApplicationFactory<Program> _factory;
  readonly HttpClient _client; 

  HttpResponseMessage? _response;

    readonly IMongoCollection<BsonDocument> _smartLinksCollection;

  public NotFoundStepDefinitions(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = _factory.CreateClient(new WebApplicationFactoryClientOptions{ AllowAutoRedirect = false,});
    _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 

    var mongoDBSettings = _factory.Services.GetService<IConfiguration>()!.GetSection("RedirectorMongoDB")!.Get<MongoDBSettings>();
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    _smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
  }
  
  // Функция: Приложение отвечает 404 Not Found ошибкой на GET запрос,
  // если для запрашиваемой умной ссылки не определены правила редиректа

  [Given("Для умной ссылки /non-exists не определены правила редиректа")]
  public void Given_Redirect_Rules_Are_Not_Defined_For_non_exists_SmartLink()
  {
  }
 
  [When("Клиент отправляет GET-запрос на url /non-exists")]
  public async Task A_Client_Sends_Get_Request_On_Url_non_exists()
  {
    _response = await _client!.GetAsync("/non-exists");
  }
  
  [Then("Приложение отвечает 404 Not Found ошибкой")]
  public void The_App_Resonses_With_404_Not_Found_Error()
  {
    Assert.Equal(404, (int) _response!.StatusCode);
  }

  // Определение шагов завершено

  // Функция: Приложение не отвечает 404 Not Found ошибкой на GET запрос,
  // если для запрашиваемой умной ссылки определены правила редиректа

  [Given("Для умной ссылки /exists определены правила редиректа")]
  public async Task Given_Redirect_Rules_Are_Defined_For_exists_SmartLink()
  {
    await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/exists\" }"));
  }
 
  [When("Клиент отправляет GET-запрос на url /exists")]
  public async Task A_Client_Sends_Get_Request_On_Url_exists()
  {
    _response = await _client!.GetAsync("/exists");
  }
  
  [Then("Приложение не отвечает 404 Not Found ошибкой")]
  public void The_App_Resonses_With_Not_404_Not_Found_Error()
  {
    Assert.NotEqual(404, (int) _response!.StatusCode);
  }

  // Определение шагов завершено

  // Сценарий: Приложение отвечает 404 Not Found ошибкой на GET запрос,
  // если для запрашиваемой умной ссылки правила редиректа были помечены как удаленные

  [Given("Для умной ссылки /deleted правила редиректа были помечены как удаленные")]
  public async Task Given_Redirect_Rules_Are_Marked_As_Deleted_For_deleted_SmartLink()
  {
    await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/deleted\", \"state\": \"deleted\" }"));
  }
 
  [When("Клиент отправляет GET-запрос на url /deleted")]
  public async Task A_Client_Sends_Get_Request_On_Url_deleted()
  {
    _response = await _client!.GetAsync("/deleted");
  }
  
  public void Dispose()
  {
    _client.Dispose();
  }
}
