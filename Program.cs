using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Enums;
using MinimalApiProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

app.UseCors("Acesso Total");

// endpoints de usuarios
app.MapPost("/usuario", async (Usuario usuario, AppDbContext context) =>
{
    context.Add(usuario);
    await context.SaveChangesAsync();

    return Results.Created($"/usuario/{usuario.Id}", usuario);
});

app.MapGet("/usuario", async (AppDbContext context) =>
{
    var usuarios = await context.Usuario.ToListAsync();

    if (usuarios is null)
    {
        return Results.NotFound("Usu�rio n�o encontrado");
    }

    return Results.Ok(usuarios);
});

app.MapGet("/usuario/{id}", async (int id, AppDbContext context) =>
{
    var usuario = await context.Usuario
        .FirstOrDefaultAsync(u => u.Id == id);

    if (usuario is null)
    {
        return Results.NotFound("Usu�rio n�o encontrado");
    }

    return Results.Ok(usuario);
});

app.MapPut("/usuario/{id}", async (int id, Usuario usuario, AppDbContext context) =>
{
    var usuarioExistente = await context.Usuario.FindAsync(id);

    if (usuarioExistente is null)
    {
        return Results.NotFound("Usu�rio n�o encontrado");
    }

    usuarioExistente.Nome = usuario.Nome;
    usuarioExistente.Email = usuario.Email;

    await context.SaveChangesAsync();

    return Results.Ok("Usu�rio atualizado com sucesso");
});

app.MapDelete("/usuario/{id}", async (int id, AppDbContext context) =>
{
    var usuario = await context.Usuario.FindAsync(id);
   
    if (usuario is null)
    {
        return Results.NotFound("Usu�rio n�o encontrado");
    }

    context.Remove(usuario);
    await context.SaveChangesAsync();

    return Results.Ok("Usu�rio deletado com sucesso");
});

// endpoints de tarefas
app.MapPost("/tarefa", async (Tarefa tarefa, AppDbContext context) =>
{
    var usuario = await context.Usuario.FindAsync(tarefa.UsuarioId);
    if (usuario is null)
        return Results.BadRequest("Usu�rio n�o encontrado");

    tarefa.DataCriacao = DateTime.Now;
    tarefa.Status = StatusTarefa.Pendente;

    context.Add(tarefa);
    await context.SaveChangesAsync();

    return Results.Created($"/tarefa/{tarefa.Id}", tarefa);
});

app.MapGet("/tarefa", async (AppDbContext context) =>
{
    var tarefas = await context.Tarefa
        .Include(t => t.Usuario)
        .ToListAsync();

    if (tarefas is null || tarefas.Count == 0)
        return Results.NotFound("Nenhuma tarefa encontrada");

    return Results.Ok(tarefas);
});

app.MapGet("/tarefa/{id}", async (int id, AppDbContext context) =>
{
    var tarefa = await context.Tarefa
        .Include(t => t.Usuario)
        .FirstOrDefaultAsync(t => t.Id == id);

    if (tarefa is null)
        return Results.NotFound("Tarefa n�o encontrada");

    return Results.Ok(tarefa);
});

app.MapGet("/tarefa/usuario/{id}", async (int id, AppDbContext context) =>
{
    var tarefas = await context.Tarefa
        .Include(t => t.Usuario)
        .Where(t => t.UsuarioId == id)
        .ToListAsync();

    return tarefas.Any() 
        ? Results.Ok(tarefas)
        : Results.NotFound("Nenhuma tarefa encontrada para esse usuário");
});

app.MapPut("/tarefa/{id}", async (int id, Tarefa tarefaAtualizada, AppDbContext context) =>
{
    var tarefa = await context.Tarefa.FindAsync(id);

    if (tarefa is null)
        return Results.NotFound("Tarefa n�o encontrada");

    tarefa.Titulo = tarefaAtualizada.Titulo;
    tarefa.Descricao = tarefaAtualizada.Descricao;
    tarefa.Status = tarefaAtualizada.Status;
    tarefa.DataConclusao = tarefaAtualizada.DataConclusao;

    await context.SaveChangesAsync();
    return Results.Ok(tarefa);
});

app.MapDelete("/tarefa/{id}", async (int id, AppDbContext context) =>
{
    var tarefa = await context.Tarefa.FindAsync(id);

    if (tarefa is null)
        return Results.NotFound("Tarefa n�o encontrada");

    context.Tarefa.Remove(tarefa);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("/tarefa/concluir/{id}", async (int id, AppDbContext context) =>
{
    var tarefa = await context.Tarefa.FindAsync(id);

    if (tarefa is null)
        return Results.NotFound("Tarefa n�o encontrada");

    tarefa.Status = StatusTarefa.Concluido;

    await context.SaveChangesAsync();

    return Results.Ok("Voc� concluiu a tarefa " + tarefa.Titulo);
});

app.Run();
