using System;
using System.IO;

namespace CatAndMouseCSharp;

internal static class Program
{
    private static void Main()
    {
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string inputPath = Path.Combine(currentDirectory, "ChaseData.txt");

            // Also check the application folder, which can be useful when launched from an IDE.
            if (!File.Exists(inputPath))
            {
                inputPath = Path.Combine(AppContext.BaseDirectory, "ChaseData.txt");
            }

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Файл ChaseData.txt не найден.");
                Console.WriteLine("Поместите его в папку проекта и запустите программу ещё раз.");
                return;
            }

            string[] inputLines = File.ReadAllLines(inputPath);
            if (inputLines.Length == 0)
            {
                Console.WriteLine("Файл ChaseData.txt пуст.");
                return;
            }

            if (!int.TryParse(inputLines[0].Trim(), out int boardSize) || boardSize < 1 || boardSize > 10000)
            {
                Console.WriteLine("Первая строка должна содержать размер поля от 1 до 10000.");
                return;
            }

            Game game = new Game(boardSize);
            game.Run(inputLines);

            string inputDirectory = Path.GetDirectoryName(Path.GetFullPath(inputPath));
            string outputPath = Path.Combine(inputDirectory, "PursuitLog.txt");
            game.WriteReport(outputPath);

            Console.WriteLine("Обработка завершена.");
            Console.WriteLine("Результат сохранён в файл: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обработке файла: " + ex.Message);
        }
    }
}
