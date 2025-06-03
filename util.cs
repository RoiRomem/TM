using Console = System.Console;

namespace TM;

public static class util
{
    public static void WriteHighlight(string text)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static string GetTaskFormat(TaskData task, int index) =>
        $"{index} | {(task.Done ? "[✓]" : "[ ]")} | {task.Task} {(task.FinishDate != null ? $"| {task.FinishDate}" : "")}";

    public static void WriteList(List<TaskData> tasks, int index)
    {
        var _index = 0;
        foreach (var task in tasks)
        {
            if (_index == index) WriteHighlight(GetTaskFormat(task, index));
            else
            {
                Console.WriteLine(GetTaskFormat(task, _index));
            }
            _index++;
        }

        if (_index != 0) return;

        WriteHighlight("It's pretty empty in here - press 'a' to add a new item");
    }

    public static string GetTextInput()
    {
        var bottomLine = Console.WindowHeight - 1;

        Console.SetCursorPosition(0, bottomLine);

        Console.Write(new string(' ', Console.WindowWidth));

        Console.SetCursorPosition(0, bottomLine);

        Console.Write(":");

        return Console.ReadLine() ?? "";
    }

    public static string GetTextInput(string prompt)
    {
        var bottomLine = Console.WindowHeight - 1;
        var oneAbove = bottomLine - 1;

        Console.SetCursorPosition(0, bottomLine);

        Console.Write(new string(' ', Console.WindowWidth));

        Console.SetCursorPosition(0, oneAbove);

        Console.Write(new string(' ', Console.WindowWidth));

        Console.SetCursorPosition(0, oneAbove);
        Console.Write(prompt);

        Console.SetCursorPosition(0, bottomLine);

        Console.Write(":");

        return Console.ReadLine() ?? "";
    }

    public static TaskData? MakeNewTask()
    {
        var task = GetTextInput("Enter New Task Name");
        if (string.IsNullOrEmpty(task)) return null;

        var dueDate = GetTextInput("Enter Due Date");
        if (string.IsNullOrEmpty(dueDate)) return new(task, false);

        return new(task, false, dueDate);
    }
}