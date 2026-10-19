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

            RandomizePlayerValues(inputLines, boardSize);

            Game game = new Game(boardSize);
            game.Run(inputLines);

            string inputDirectory = Path.GetDirectoryName(Path.GetFullPath(inputPath));
            string outputPath = Path.Combine(inputDirectory, "PursuitLog.txt");
            game.WriteReport(outputPath);

            foreach (string reportLine in File.ReadAllLines(outputPath))
            {
                Console.WriteLine(reportLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обработке файла: " + ex.Message);
        }
    }

    private static void RandomizePlayerValues(string[] inputLines, int boardSize)
    {
        bool mouseHasPosition = false;
        bool catHasPosition = false;

        for (int i = 1; i < inputLines.Length; i++)
        {
            string line = inputLines[i].Trim();
            if (line.Length == 0)
            {
                continue;
            }

            string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToUpperInvariant();

            if (command == "M")
            {
                int value = mouseHasPosition
                    ? Random.Shared.Next(-boardSize, boardSize + 1)
                    : Random.Shared.Next(1, boardSize + 1);
                inputLines[i] = "M " + value;
                mouseHasPosition = true;
            }
            else if (command == "C")
            {
                int value = catHasPosition
                    ? Random.Shared.Next(-boardSize, boardSize + 1)
                    : Random.Shared.Next(1, boardSize + 1);
                inputLines[i] = "C " + value;
                catHasPosition = true;
            }
        }
    }
}

