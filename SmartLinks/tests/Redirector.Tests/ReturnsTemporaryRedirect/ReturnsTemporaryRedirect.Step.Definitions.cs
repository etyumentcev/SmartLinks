namespace Redirector.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

using MongoDB.Driver;
using TechTalk.SpecFlow;

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
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });
        _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 

        var mongoDBSettings = _factory.Services.GetService<IConfiguration>()!.GetSection("RedirectorMongoDB")!.Get<MongoDBSettings>();
        var client = new MongoClient(mongoDBSettings!.ConnectionURI);
        var database = client.GetDatabase(mongoDBSettings.DatabaseName);
        _smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
    }

    // Функция: Приложение отвечает 307 Temporary Redirect на GET запрос, 
    // если для запрашиваемой умной ссылки определено одно безусловное правило редиректа

    [Given("Для умной ссылки /unconditional-redirect определено одно безусловное правило редиректа")]
    public async void Given_One_Redirect_Rule_Is_Defined_For_unconidtional_redirect_SmartLink()
    {
        await _smartLinksCollection.InsertOneAsync(
          BsonDocument.Parse(
            "{ slug: \"/unconditional-redirect\", rules: [{redirectTo: \"hwdtech.ru\"}]}"
        ));
    }

    [When("Клиент отправляет GET-запрос на url /unconditional-redirect")]
    public async Task A_Client_Sends_Get_Request_On_Url_uncoditional_redirect()
    {
        _response = await _client!.GetAsync("/unconditional-redirect");
    }

    [Then("Приложение отвечает 307 Temporary Redirect")]
    public void The_App_Resonses_With_307_Temporary_Redirect()
    {
        Assert.Equal(307, (int)_response!.StatusCode);
        Assert.Equal("hwdtech.ru", _response.Headers.Location!.ToString());
    }

    [Then("Приложение отвечает 429 Unprocessable Content")]
    public void The_App_Resonses_With_429_Unprocessable_Content_Error()
    {
        Assert.Equal(429, (int)_response!.StatusCode);
    }

    [Given("Для умной ссылки /language-redirect определено правило редиректа, в котором указан язык ru-RU")]
    public async void Given_SmartLink_language_redirect_has_a_redirect_rule_with_language_condition()
    {
        await _smartLinksCollection.InsertOneAsync(
          BsonDocument.Parse(
            "{ slug: \"/language-redirect\", rules: [{language: \"ru-RU\", redirectTo: \"hwdtech.ru\"}]}"
        ));
    }

    [When("Клиент отправляет GET-запрос на url /language-redirect c accept-language en-US;q=0.9, ru-RU;q=0.8")]
    public async Task A_Client_Sends_Get_Request_On_Url_language_redirect()
    {
        _client.DefaultRequestHeaders.Add("accept-language", "en-US;q=0.9, ru-RU;q=0.8");
        _response = await _client!.GetAsync("/language-redirect");
    }

    [When("Клиент отправляет GET-запрос на url /language-redirect c accept-language en")]
    public async Task A_Client_Sends_Get_Request_On_Url_language_redirect_Witn_Language_En()
    {
        _client.DefaultRequestHeaders.Add("accept-language", "en");
        _response = await _client!.GetAsync("/language-redirect");
    }

    [When("Клиент отправляет GET-запрос на url /language-redirect c accept-language any")]
    public async Task A_Client_Sends_Get_Request_On_Url_language_redirect_With_Any_Language()
    {
        _client.DefaultRequestHeaders.Add("accept-language", "*");
        _response = await _client!.GetAsync("/language-redirect");
    }

    [Given("Для умной ссылки /language-redirect1 определено правило редиректа, в котором указан язык any")]
    public async void Given_SmartLink_language_redirect1_has_a_redirect_rule_with_language_condition_any()
    {
        await _smartLinksCollection.InsertOneAsync(
          BsonDocument.Parse(
            "{ slug: \"/language-redirect1\", rules: [{language: \"any\", redirectTo: \"hwdtech.ru\"}]}"
        ));
    }

    [When("Клиент отправляет GET-запрос на url /language-redirect1 c accept-language en-US;q=0.9, ru-RU;q=0.8")]
    public async Task A_Client_Sends_Get_Request_On_Url_language_redirect1()
    {
        _client.DefaultRequestHeaders.Add("accept-language", "en-US;q=0.9, ru-RU;q=0.8");
        _response = await _client!.GetAsync("/language-redirect1");
    }

    [Given("Для умной ссылки /time-redirect определено правило редиректа, в котором указан временной интервал включаю текущую дату")]
    public async void Given_SmartLink_time_redirect_has_a_redirect_rule_with_time_interval_condition()
    {
        var current = DateTime.UtcNow;
        var start = current.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssK");
        var end = current.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssK");
        var record = $"{{ slug: \"/time-redirect\", rules: [{{start: \"{start}\", end: \"{end}\", redirectTo: \"hwdtech.ru\"}}]}}";
        await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse(record));
    }

    [When("Клиент отправляет GET-запрос на url /time-redirect")]
    public async Task A_Client_Sends_Get_Request_On_Url_time_redirect()
    {
        _response = await _client!.GetAsync("/time-redirect");
    }

    [Given("Для умной ссылки /multi-redirect определены два правила редиректа")]
    public async void Given_SmartLink_multi_redirect_has_two_redirect_rules()
    {
        var current = DateTime.UtcNow;
        var start = current.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssK");
        var end = current.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssK");
        var record = $"{{ slug: \"/multi-redirect\", rules: [{{start: \"{start}\", end: \"{end}\", language: \"en\", redirectTo: \"google.com\"}}, {{language: \"ru\", redirectTo: \"hwdtech.ru\"}}]}}";
        await _smartLinksCollection.InsertOneAsync(BsonDocument.Parse(record));
    }

    [When("Клиент отправляет GET-запрос на url /multi-redirect c accept-language ru")]
    public async Task A_Client_Sends_Get_Request_On_Url_multi_redirect()
    {
        _client.DefaultRequestHeaders.Add("accept-language", "ru");
        _response = await _client!.GetAsync("/multi-redirect");
    }

    public void Dispose()
    {
        _client.Dispose();
    }


}


