using Redirector;
using Microsoft.Extensions.Options;


using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoDBSettings = builder.Configuration.GetSection("RedirectorMongoDB").Get<MongoDBSettings>();

builder.Services.AddScoped<ISmartLink, ImplSmartLink>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{ 
  return new MongoClient(mongoDBSettings!.ConnectionURI);
});
builder.Services.AddSingleton<IMongoCollection<BsonDocument>>(sp =>
{
  var client = sp.GetRequiredService<IMongoClient>();
  var database = client.GetDatabase(mongoDBSettings!.DatabaseName);
  return database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
});
//Register ReturnsNotFoundFeature
builder.Services.AddTransient<The_App_Responses_404_Not_Found_Middleware>();
// Attach ReturnsNotFoundFacadeForHttp 
builder.Services.AddScoped<ISupportedHttpRequest, ImplSupportedHttpRequest>();


//Register ReturnsUnprocessableContentFeature
builder.Services.AddTransient<The_App_Responses_422_Uprocessable_Content_Middleware>();
// Attach ReturnsUnprocessableContentFacadeForRepository
builder.Services.AddScoped<IFreezeSmartLinkService, ImplFreezeSmartLinkService>();

builder.Services.AddTransient<The_App_Responses_405_Method_Not_Allowed_Middleware>();
builder.Services.AddTransient<The_App_Responses_307_Temporary_Redirect_Middleware>();



// Add the dependency from IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMongoDbRepository, ImplMongoDbRepository>();
builder.Services.AddScoped<IStatableSmartLinkRepository, ImplStatableSmartLinkRepository>();

builder.Services.AddScoped<ISmartLinkRedirectService, ImplSmartLinkRedirectService>();
builder.Services.AddScoped<ISmartLinkRedirectRulesRepository, ImplSmartLinkRedirectRulesRepository>();
builder.Services.AddScoped<IRedirectableSmartLink, ImplRedirectableSmartLink>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<The_App_Responses_405_Method_Not_Allowed_Middleware>();
// Attach ReturnsNotFoundFeature 
app.UseMiddleware<The_App_Responses_404_Not_Found_Middleware>();
// Attach ReturnsUnprocessableContentFeature
app.UseMiddleware<The_App_Responses_422_Uprocessable_Content_Middleware>();

app.UseMiddleware<The_App_Responses_307_Temporary_Redirect_Middleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    var smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);

    await smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ someField: 1 }"));
}


app.Run();

public partial class Program { }

