using System;

namespace TM
{
    public static class main
    {
        public static void Main(string[] args)
        {
            Console.Clear();

            Console.WriteLine("TM - Task Manager");
            var tasks = DataManager.FetchData();

            bool active = true;

            var currentIndex = 0;

            util.WriteList(tasks, currentIndex);

            while (active)
            {
                // Wait for input
                ManageInput(Console.ReadKey(intercept: true), ref active, ref currentIndex, ref tasks);
                // Clear screen
                Console.Clear();

                // Writing to screen
                Console.WriteLine("TM - Task Manager");
                util.WriteList(tasks, currentIndex);
            }

            // write data before closing
            DataManager.WriteData(tasks);
        }

        private static void ManageInput(ConsoleKeyInfo iKey, ref bool active, ref int currentIndex, ref List<TaskData> tasks)
        {
            switch (iKey.KeyChar)
            {
                case 'a':
                    var nTask = util.MakeNewTask();
                    if(nTask != null) tasks.Add(nTask);
                    currentIndex = tasks.Count - 1;
                    break;

                case 'j':
                    if (currentIndex + 1 == tasks.Count) currentIndex = 0;
                    else currentIndex++;
                    break;

                case 'k':
                    if (currentIndex - 1 == -1) currentIndex = tasks.Count - 1;
                    else currentIndex--;
                    break;

                case 'q':
                    active = false;
                    break;

                case 's':
                    tasks[currentIndex].Mark();
                    break;

                case 'd':
                    if (tasks.Count == 0) break;
                    tasks.RemoveAt(currentIndex);
                    if (currentIndex >= tasks.Count) currentIndex = tasks.Count - 1;
                    break;
            }
        }
    }
}