using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure RavenDB
var ravenSettings = builder.Configuration.GetSection("RavenDB");
var store = new DocumentStore
{
    Urls = ravenSettings.GetSection("Urls").Get<string[]>() ?? ["http://localhost:8080"],
    Database = ravenSettings.GetValue<string>("Database") ?? "Demo"
};
store.Initialize();
builder.Services.AddSingleton<IDocumentStore>(store);

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

app.Run();
