namespace TM;

public class TaskData
{
    public string Task { get; set; }
    public bool Done { get; set; }
    public string? FinishDate { get; set; }

    public TaskData() {}

    public TaskData(string task, bool done, string? finishDate)
    {
        Task = task;
        Done = done;
        FinishDate = finishDate;
    }

    public TaskData(string task, bool done)
    {
        Task = task;
        Done = done;
    }

    public void Mark() => Done = !Done;
}