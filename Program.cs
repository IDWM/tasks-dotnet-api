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

static async Task<IResult> GetAllTasks(DataContext db)
{
    return TypedResults.Ok(await db.Tasks.ToArrayAsync());
}

static async Task<IResult> GetCompleteTasks(DataContext db)
{
    return TypedResults.Ok(await db.Tasks.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetTask(int id, DataContext db)
{
    return await db.Tasks.FindAsync(id) is Task task
        ? TypedResults.Ok(task)
        : TypedResults.NotFound();
}

static async Task<IResult> CreateTask(Task task, DataContext db)
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/tasks/{task.Id}", task);
}

static async Task<IResult> UpdateTask(int id, Task inputTask, DataContext db)
{
    var task = await db.Tasks.FindAsync(id);

    if (task is null)
        return TypedResults.NotFound();

    task.Name = inputTask.Name;
    task.IsComplete = inputTask.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTask(int id, DataContext db)
{
    if (await db.Tasks.FindAsync(id) is Task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
