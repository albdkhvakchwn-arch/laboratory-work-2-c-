using System;

namespace CatAndMouseCSharp;

internal sealed class Player
{
    public string Name { get; }
    public int Position { get; private set; }
    public PlayerState State { get; private set; }
    public long DistanceTraveled { get; private set; }

    public Player(string name)
    {
        Name = name;
        Position = 0;
        State = PlayerState.NotInGame;
        DistanceTraveled = 0;
    }

    // The first command gives the starting position; it is not counted as movement.
    public void Move(int value, int boardSize)
    {
        if (State == PlayerState.NotInGame)
        {
            Position = value;
            State = PlayerState.Playing;
            return;
        }

        // Keep the position in the range 1..boardSize, including for negative moves.
        long newPosition = ((Position - 1L + value) % boardSize + boardSize) % boardSize + 1L;
        Position = (int)newPosition;
        DistanceTraveled += Math.Abs((long)value);
    }

    public void SetState(PlayerState state)
    {
        State = state;
    }
}
