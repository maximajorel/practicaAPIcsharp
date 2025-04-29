var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Servidor en funcionamiento");

app.MapGet("/tareas/{id:int}", (int id) =>
{

    return Results.Ok($"Se ha solicitado la tarea con id {id}");

});


app.MapGet("/tareas/", (int id) =>
{

    return Results.Ok($"Se ha solicitado la tarea con id {id}");

});


app.MapDelete("/tareas/{id:int}", (int id) =>
{
    return Results.Ok($"Se eliminó la tarea con ID {id}");
});


app.MapPost("/tareas", (Tarea nuevo) =>
{

    return Results.Ok($"Se ha creado la tarea {nuevo.Titulo} con id {nuevo.Id}, su estado inicial es {nuevo.Completada}");

});

app.Run();


record Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public bool Completada { get; set; }
}