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

    public static void WriteList(List<TaskData> tasks, int selectedIndex)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (i == selectedIndex) 
                WriteHighlight(GetTaskFormat(tasks[i], i + 1));
            else
                Console.WriteLine(GetTaskFormat(tasks[i], i + 1));
        }

        if (tasks.Count == 0)
        {
            WriteHighlight("It's pretty empty in here - press 'a' to add a new item");
        }
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

        // Clear both lines
        Console.SetCursorPosition(0, bottomLine);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, oneAbove);
        Console.Write(new string(' ', Console.WindowWidth));

        // Write prompt on line above
        Console.SetCursorPosition(0, oneAbove);
        Console.Write(prompt);

        // Position cursor for input
        Console.SetCursorPosition(0, bottomLine);
        Console.Write(":");

        return Console.ReadLine() ?? "";
    }

    public static TaskData? MakeNewTask()
    {
        var task = GetTextInput("Enter New Task Name");
        if (string.IsNullOrEmpty(task)) return null;

        var dueDate = GetTextInput("Enter Due Date (optional)");
        if (string.IsNullOrEmpty(dueDate)) 
            return new TaskData(task, false);

        return new TaskData(task, false, dueDate);
    }

    private static void PrintQuestion(string buffer, string prompt, int cursor)
    {
        var bottomLine = Console.WindowHeight - 1;
        var oneAbove = bottomLine - 1;

        // Clear both lines
        Console.SetCursorPosition(0, bottomLine);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, oneAbove);
        Console.Write(new string(' ', Console.WindowWidth));
        
        // Write prompt and buffer
        Console.SetCursorPosition(0, oneAbove);
        Console.Write(prompt);
        Console.Write(buffer);
    }

    private static void PrintGrep(string buffer, int cursor)
    {
        var bottomLine = Console.WindowHeight - 1;
        
        Console.SetCursorPosition(0, bottomLine);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, bottomLine);
        Console.Write("/");
        Console.Write(buffer);
        Console.SetCursorPosition(buffer.Length + 1, bottomLine);
    }
    
    public static string PlaceHolder(string initialBuffer, string prompt)
    {
        var buffer = initialBuffer;
        var cursor = buffer.Length;

        PrintQuestion(buffer, prompt, cursor);
        Console.SetCursorPosition(cursor + prompt.Length, Console.WindowHeight - 1);
        
        var key = Console.ReadKey(intercept: true);
        while (key.Key != ConsoleKey.Enter)
        {
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (cursor > 0)
                    {
                        buffer = buffer.Remove(cursor - 1, 1);
                        cursor--;
                    }
                    break;
                
                case ConsoleKey.Delete:
                    if (cursor < buffer.Length)
                    {
                        buffer = buffer.Remove(cursor, 1);
                    }
                    break;
                
                case ConsoleKey.LeftArrow:
                    if (cursor > 0) cursor--;
                    break;
                
                case ConsoleKey.RightArrow:
                    if (cursor < buffer.Length) cursor++;
                    break;
                
                default:
                    if (!char.IsControl(key.KeyChar))
                    {
                        buffer = buffer.Insert(cursor, key.KeyChar.ToString());
                        cursor++;
                    }
                    break;
            }
            
            PrintQuestion(buffer, prompt, cursor);
            Console.SetCursorPosition(cursor + prompt.Length, Console.WindowHeight - 1);
            key = Console.ReadKey(intercept: true);
        }
        return buffer;
    }

    /// <summary>
    /// Find first task containing the search term (case-insensitive)
    /// </summary>
    /// <param name="tasks">Task list to search</param>
    /// <param name="searchTerm">Term to search for within task names</param>
    /// <returns>The index of the first matching task (-1 if not found)</returns>
    private static int GetFirstMatchingTask(List<TaskData> tasks, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return -1;
        
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].Task.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }
    
    public static void Grep(List<TaskData> tasks, ref int index)
    {
        var buffer = "";
        var cursor = 0;
        var originalIndex = index;

        PrintGrep(buffer, cursor);
        var key = Console.ReadKey(intercept: true);

        while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
        {
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (cursor > 0)
                    {
                        buffer = buffer.Remove(cursor - 1, 1);
                        cursor--;
                    }
                    break;

                case ConsoleKey.Delete:
                    if (cursor < buffer.Length)
                        buffer = buffer.Remove(cursor, 1);
                    break;

                case ConsoleKey.LeftArrow:
                    if (cursor > 0) cursor--;
                    break;

                case ConsoleKey.RightArrow:
                    if (cursor < buffer.Length) cursor++;
                    break;

                default:
                    if (!char.IsControl(key.KeyChar))
                    {
                        buffer = buffer.Insert(cursor, key.KeyChar.ToString());
                        cursor++;
                    }
                    break;
            }

            // Update the selected index based on search
            var matchIndex = GetFirstMatchingTask(tasks, buffer);
            index = matchIndex != -1 ? matchIndex : originalIndex;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("TM - Task Manager");
            util.WriteList(tasks, index);
            
            PrintGrep(buffer, cursor);
            Console.SetCursorPosition(cursor + 1, Console.WindowHeight - 1); // +1 for the "/" character
            
            key = Console.ReadKey(intercept: true);
        }
        
        // If user pressed Escape, restore original index
        if (key.Key == ConsoleKey.Escape) index = originalIndex;
    }
}