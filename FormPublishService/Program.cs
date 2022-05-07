var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("FormDraftService", o =>
{
    o.BaseAddress = new Uri(builder.Configuration["FormDraftServiceBaseAddress"]);
});
builder.Services.AddHttpClient("SecretService", o =>
{
    o.BaseAddress = new Uri(builder.Configuration["SecretServiceBaseAddress"]);
});


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
