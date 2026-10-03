using NovaStore.Api.Endpoints;
using NovaStore.Api.ExceptionHandling;
using NovaStore.Application;
using NovaStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapProductsEndpoints();
app.MapCategoriesEndpoints();
app.MapBrandsEndpoints();

app.Run();