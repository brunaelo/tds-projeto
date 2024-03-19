using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTDS.Context;
using ProjetoTDS.Models;

var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Produto 1", Descricao = "Descrição do Produto 1", Preco = 10.5m, Quantidade = 100 },
    new Produto { Id = 2, Nome = "Produto 2", Descricao = "Descrição do Produto 2", Preco = 20.75m, Quantidade = 50 }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=Database.db"));
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


app.MapGet("/produtos", async (DatabaseContext database) =>
{
    return await database.Produtos.ToListAsync();
});

app.MapGet("/produtos/{id}", async (DatabaseContext database, int id) =>
{
    var produto = await database.Produtos.SingleOrDefaultAsync(p => p.Id == id);

    if (produto != null)
    {
        return Results.Ok(produto);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/produtos", async (DatabaseContext database, [FromBody] Produto produto) =>
{
    await database.Produtos.AddAsync(produto);
    await database.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/produtos/{id}", async (HttpContext context, DatabaseContext database, int id, Produto produto) =>
{
    var produtoExistente = await database.Produtos.FirstOrDefaultAsync(p => p.Id == id);
    if (produtoExistente != null)
    {
        produtoExistente.Nome = produto.Nome;
        produtoExistente.Descricao = produto.Descricao;
        produtoExistente.Preco = produto.Preco;
        produtoExistente.Quantidade = produto.Quantidade;
        await database.SaveChangesAsync();
        return Results.Ok(produtoExistente);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/produtos/{id}", async (DatabaseContext database, int id) =>
{
    var produto = await database.Produtos.FirstOrDefaultAsync(p => p.Id == id);

    if (produto != null)
    {
        database.Produtos.Remove(produto);
        await database.SaveChangesAsync();
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();

