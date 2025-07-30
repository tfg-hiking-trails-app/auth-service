using AuthService.API.Extensions;
using AuthService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureServicesCollection();

var app = builder.Build();

app.UseMiddleware<InternalErrorMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
