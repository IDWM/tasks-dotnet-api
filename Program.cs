using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("TaskList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var tasks = app.MapGroup("/task");

tasks.MapGet("/", GetAllTasks);
tasks.MapGet("/complete", GetCompleteTasks);
tasks.MapGet("/{id}", GetTask);
tasks.MapPost("/", CreateTask);
tasks.MapPut("/{id}", UpdateTask);
tasks.MapDelete("/{id}", DeleteTask);

app.Run();

static IResult GetAllTasks(DataContext db)
{
    return TypedResults.Ok(db.Tasks.ToArray());
}

static IResult GetCompleteTasks(DataContext db)
{
    return TypedResults.Ok(db.Tasks.Where(t => t.IsComplete).ToList());
}

static IResult GetTask(int id, DataContext db)
{
    return db.Tasks.Find(id) is Task task ? TypedResults.Ok(task) : TypedResults.NotFound();
}

static IResult CreateTask(Task task, DataContext db)
{
    db.Tasks.Add(task);
    db.SaveChanges();

    return TypedResults.Created($"/tasks/{task.Id}", task);
}

static IResult UpdateTask(int id, Task inputTask, DataContext db)
{
    var task = db.Tasks.Find(id);

    if (task is null)
        return TypedResults.NotFound();

    task.Name = inputTask.Name;
    task.IsComplete = inputTask.IsComplete;

    db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static IResult DeleteTask(int id, DataContext db)
{
    if (db.Tasks.Find(id) is Task task)
    {
        db.Tasks.Remove(task);
        db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
