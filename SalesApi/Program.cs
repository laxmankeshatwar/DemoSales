using SalesApi.Models;
using SalesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ISalesRepository, SalesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var sales = app.MapGroup("/api/sales");

sales.MapGet("/", (ISalesRepository repo) => Results.Ok(repo.GetAll()));

sales.MapGet("/{id:int}", (int id, ISalesRepository repo) =>
{
    var sale = repo.Get(id);
    return sale is null ? Results.NotFound() : Results.Ok(sale);
});

sales.MapPost("/", (SaleCreate create, ISalesRepository repo, HttpContext http) =>
{
    var created = repo.Create(create);
    var uri = $"/api/sales/{created.Id}";
    return Results.Created(uri, created);
});

sales.MapPut("/{id:int}", (int id, SaleUpdate update, ISalesRepository repo) =>
{
    var ok = repo.Update(id, update);
    return ok ? Results.NoContent() : Results.NotFound();
});

sales.MapDelete("/{id:int}", (int id, ISalesRepository repo) =>
{
    var ok = repo.Delete(id);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.Run();

