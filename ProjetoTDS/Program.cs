using Microsoft.AspNetCore.Mvc;
using ProjetoTDS.Models;

var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Produto 1", Descricao = "Descrição do Produto 1", Preco = 10.5m, Quantidade = 100 },
    new Produto { Id = 2, Nome = "Produto 2", Descricao = "Descrição do Produto 2", Preco = 20.75m, Quantidade = 50 }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();


app.MapGet("/produtos", () =>
{
    return produtos.ToList();
});

app.MapGet("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto != null)
    {
        return Results.Ok(produto);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/produtos", ([FromBody] Produto produto) =>
{
    produtos.Add(produto);
    return TypedResults.CreatedAtRoute();
});

app.MapPut("/produtos/{id}", (HttpContext context, int id, Produto produto) =>
{
    var produtoExistente = produtos.FirstOrDefault(p => p.Id == id);
    if (produtoExistente != null)
    {
        produtoExistente.Nome = produto.Nome;
        produtoExistente.Descricao = produto.Descricao;
        produtoExistente.Preco = produto.Preco;
        produtoExistente.Quantidade = produto.Quantidade;
        return Results.Ok(produtoExistente);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto != null)
    {
        produtos.Remove(produto);
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();

