using System;
using System.Collections.Generic;
using System.IO;

namespace CatAndMouseCSharp;

internal sealed class Game
{
    private readonly int boardSize;
    private readonly List<string> tableRows;
    private bool mouseCaught;
    private int caughtCell;

    public Player Cat { get; }
    public Player Mouse { get; }

    public Game(int size)
    {
        boardSize = size;
        Cat = new Player("Cat");
        Mouse = new Player("Mouse");
        tableRows = new List<string>();
        mouseCaught = false;
        caughtCell = 0;
    }

    public void Run(string[] inputLines)
    {
        // The first line contains the size of the board.
        for (int i = 1; i < inputLines.Length; i++)
        {
            string line = inputLines[i].Trim();
            if (line.Length == 0)
            {
                continue;
            }

            string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToUpperInvariant();

            if (command == "P")
            {
                SaveTableRow();
                continue;
            }

            int value = int.Parse(parts[1]);

            if (command == "M")
            {
                Mouse.Move(value, boardSize);
            }
            else if (command == "C")
            {
                Cat.Move(value, boardSize);
            }
            else
            {
                continue;
            }

            CheckForCapture();
            if (mouseCaught)
            {
                // Stop reading input once the mouse is caught.
                break;
            }
        }
    }

    private void CheckForCapture()
    {
        if (Cat.State != PlayerState.NotInGame &&
            Mouse.State != PlayerState.NotInGame &&
            Cat.Position == Mouse.Position)
        {
            mouseCaught = true;
            caughtCell = Cat.Position;
            Cat.SetState(PlayerState.Winner);
            Mouse.SetState(PlayerState.Loser);
        }
    }

    private void SaveTableRow()
    {
        string catPosition = Cat.State == PlayerState.NotInGame ? "??" : Cat.Position.ToString();
        string mousePosition = Mouse.State == PlayerState.NotInGame ? "??" : Mouse.Position.ToString();
        string distance = "??";

        if (Cat.State != PlayerState.NotInGame && Mouse.State != PlayerState.NotInGame)
        {
            // The assignment's example defines Distance as the difference between cell numbers.
            distance = Math.Abs(Cat.Position - Mouse.Position).ToString();
        }

        tableRows.Add(catPosition.PadRight(10) + mousePosition.PadRight(10) + distance);
    }

    public void WriteReport(string outputPath)
    {
        if (!mouseCaught)
        {
            Cat.SetState(PlayerState.Loser);
            Mouse.SetState(PlayerState.Winner);
        }

        List<string> output = new List<string>();
        output.Add("Cat and Mouse");
        output.Add("");
        output.Add("Cat".PadRight(10) + "Mouse".PadRight(10) + "Distance");
        output.Add(new string('-', 30));

        for (int i = 0; i < tableRows.Count; i++)
        {
            output.Add(tableRows[i]);
        }

        output.Add(new string('-', 30));
        output.Add("");
        output.Add("");
        output.Add("Cat distance traveled: " + Cat.DistanceTraveled);
        output.Add("Mouse distance traveled: " + Mouse.DistanceTraveled);
        output.Add("");

        if (mouseCaught)
        {
            output.Add("Мышь поймана в клетке: " + caughtCell + " - Mouse caught at: " + caughtCell);
        }
        else
        {
            output.Add("Мышь ускользнула от кота - Mouse evaded Cat");
        }

        File.WriteAllLines(outputPath, output);
    }
}
