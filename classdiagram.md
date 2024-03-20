```mermaid
classDiagram
    class Task {
        + int Id
        + string? Name
        + bool IsComplete
    }

    class DataContext {
        + DataContext(DbContextOptions<DataContext> options)
        + Tasks: DbSet<Task>
    }

    class Program {
        + GetCompleteTasks(DataContext db) IResult$
        + GetTask(int id, DataContext db) IResult$
        + CreateTask(Task task, DTask<IResult>ataContext db) IResult$
        + UpdateTask(int id, Task inputTask, DataContext db) IResult$
        + DeleteTask(int id, DataContext db) IResult$
    }

    Program *--> "1" DataContext
    DataContext o--> "*" Task
```