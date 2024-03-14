using Redirector;

using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    var mongoDBSettings = app.Configuration.GetSection("RedirectorMongoDB").Get<MongoDBSettings>();  
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    var smartLinksCollection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);

    await smartLinksCollection.InsertOneAsync(BsonDocument.Parse("{ someField: 1 }"));
}


app.Run();
