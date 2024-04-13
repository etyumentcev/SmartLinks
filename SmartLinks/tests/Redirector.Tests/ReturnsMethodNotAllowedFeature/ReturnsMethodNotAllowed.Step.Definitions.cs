namespace Redirector.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;

using MongoDB.Driver;
using TechTalk.SpecFlow;

[Binding]
public sealed class MethodNotAllowedStepDefinitions
{
    readonly WebApplicationFactory<Program> _factory;
    readonly HttpClient _client;

    HttpResponseMessage? _response;

    readonly IMongoCollection<BsonDocument> _smartLinksCollection;

    public MethodNotAllowedStepDefinitions(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });
        _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 

        var mongoDBSettings = _factory.Services.GetService<IConfiguration>()!.GetSection("RedirectorMongoDB")!.Get<MongoDBSettings>();
        var client = new MongoClient(mongoDBSettings!.ConnectionURI);
        var database = client.GetDatabase(mongoDBSettings.DatabaseName);
        _smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
    }

    // Функция: Приложение отвечает 405 Metod Not Allowed ошибкой на POST запрос,
    // если для запрашиваемой умной ссылки не определены правила редиректа
    [Given("Для умной ссылки /post-non-exists не определены правила редиректа")]
    public void Given_Redirect_Rules_Are_Not_Defined_For_post_non_exists_SmartLink()
    {
    }

    [When("Клиент отправляет POST-запрос на url /post-non-exists")]
    public async Task A_Client_Sends_Post_Request_On_Url_non_exists()
    {
        _response = await _client!.PostAsync("/post-non-exists", new StringContent("{}"));
    }

    [Then("Приложение отвечает 405 Method Not Allowed ошибкой")]
    public void The_App_Resonses_With_405_Not_Allowed_Error()
    {
        Assert.Equal(405, (int)_response!.StatusCode);
    }

    // Определение шагов завершено

    // Функция: Приложение отвечает 405 Metod Not Allowed ошибкой на POST запрос,
    // если для запрашиваемой умной ссылки определены правила редиректа

    [Given("Для умной ссылки /post-exists определены правила редиректа")]
    public async Task Given_Redirect_Rules_Are_Defined_For_post_exists_SmartLink()
    {
        await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ slug: \"/post-exists\" }"));
    }

    [When("Клиент отправляет POST-запрос на url /post-exists")]
    public async Task A_Client_Sends_Post_Request_On_Url_post_exists()
    {
        _response = await _client!.PostAsync("/post-exists", new StringContent("{}"));
    }

    // Определение шагов завершено

    // Функция: Приложение не отвечает 405 Not Found ошибкой на GET запрос


    [When("Клиент отправляет GET-запрос на url /post-non-exists")]
    public async Task A_Client_Sends_Get_Request_On_Url_non_exists()
    {
        _response = await _client!.GetAsync("/post-non-exists");
    }

    [Then("Приложение не отвечает 405 Method Not Allowed ошибкой")]
    public void The_App_Resonses_With_Not_405_Method_Not_Allowed_Error()
    {
        Assert.NotEqual(405, (int)_response!.StatusCode);
    }
}
