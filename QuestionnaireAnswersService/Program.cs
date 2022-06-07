var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

builder.Services.AddMvc(options => options.OutputFormatters.Add(new QuestionnaireAnswersService.CustomFormatter()));

builder.Services.AddHttpClient("SecretService", o =>
{
    o.BaseAddress = new Uri(builder.Configuration["SecretServiceBaseAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
