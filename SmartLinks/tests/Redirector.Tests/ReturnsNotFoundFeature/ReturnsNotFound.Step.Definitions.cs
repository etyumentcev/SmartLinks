namespace Redirector.Tests;

using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Mvc.Testing;

[Binding]
public sealed class NotFoundStepDefinitions
{
  readonly WebApplicationFactory<Program> _factory;
  readonly HttpClient _client; 

  HttpResponseMessage? _response;

  public NotFoundStepDefinitions(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = _factory.CreateClient(new WebApplicationFactoryClientOptions{ AllowAutoRedirect = false,});
    _client.BaseAddress = new Uri("https://localhost"); //avoid https redirect warning 
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
}
