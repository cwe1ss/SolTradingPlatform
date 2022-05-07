using MeiShop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

// Das RoundRobinService muss Request-übergreifend die einzelnen Adressen durchlaufen
// und wird daher als Singleton-Service registriert.
builder.Services.AddSingleton<RoundRobinService>();

// Alternative Retry-Logik im CashDeskController könnte auch über die 3rd-Party Library "Polly" durchgeführt werden,
// mit der man zentral das Retry-Behavior über Policies steuern kann.
//builder.Services.AddHttpClient("CreditCards", client =>
//{
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//}).AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(new[]
//{
//    TimeSpan.FromSeconds(1),
//    TimeSpan.FromSeconds(5),
//    TimeSpan.FromSeconds(10),
//}));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
