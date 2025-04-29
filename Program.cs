using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () => "Servidor en funcionamiento");

var jsonTareas = "./tareas.json";
List<Tarea> tareas = File.Exists(jsonTareas) ? JsonSerializer.Deserialize<List<Tarea>>(File.ReadAllText(jsonTareas)) : new List<Tarea>();

// Mostrar todas las tareas
app.MapGet("/tareas", () =>
{

    return Results.Ok(tareas);

});


app.MapGet("/tareas/{id:int}", (int id) =>
{
    if (tareas == null)
    {
        return Results.NotFound("La lista de tareas no existe.");
    }

    var tareaSeleccionada = tareas.FirstOrDefault(t => t.Id == id);
    if (tareaSeleccionada == null)
    {
        return Results.NotFound($"No se encontró la tarea con ID {id}");
    }

    return Results.Ok(tareaSeleccionada);
});





app.MapPost("/tareas", (Tarea nuevaTarea) =>
{
    if (tareas == null)
    {
        tareas = new List<Tarea>();
    }
    if (tareas.Any(t => t.Id == nuevaTarea.Id))
    {
        return Results.Conflict($"Ya existe una tarea con ID {nuevaTarea.Id}");
    }
    tareas.Add(nuevaTarea);
    File.WriteAllText(jsonTareas, JsonSerializer.Serialize(tareas));
    return Results.Created($"/tareas/{nuevaTarea.Id}", nuevaTarea);
});

app.MapDelete("/tareas/{id:int}", (int id) =>
{
    var tareaAEliminar = tareas.FirstOrDefault(t => t.Id == id);
    if (tareaAEliminar == null)
    {
        return Results.NotFound($"No se encontró la tarea con ID {id}");
    }
    tareas.Remove(tareaAEliminar);
    File.WriteAllText(jsonTareas, JsonSerializer.Serialize(tareas));
    return Results.Ok();
});

app.Run();


record Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public bool Completada { get; set; }
}