public abstract class Monsters : Obstacles
{
    public bool IsAlive = true;
    public Monsters(int x, int y) : base(x, y)
    {
    }
}
public class Amaroks : Monsters
{
    public Amaroks(int x, int y) : base(x, y)
    {
        Description = "You found an Amarok, at least he will have something to eat";
    }

}
public class Maelstroms : Monsters
{
    public Maelstroms(int x, int y) : base(x, y)
    {
        Description = "You found an Maelstrom, things are about to get windy";
    }
    public (int, int) Encounter(int xCoor, int yCoor, int size, ICell[,] grid)
    {
        bool isMaelstromBounds = X < size - 2 && Y > 0;
        if (isMaelstromBounds)
        {
            bool isGridEmpty = grid[X + 2, Y - 1].GetType() == typeof(Empty);
            if (isGridEmpty)
            {
                X = X + 2;
                Y = Y - 1;
                if (xCoor - 2 == -1)
                    xCoor = size - 1;
                else if (xCoor - 2 == -2)
                    xCoor = size - 2;
                else
                    xCoor = xCoor - 2;

                if (yCoor + 1 == size)
                    yCoor = 0;
                else
                    yCoor = yCoor + 1;
                return (xCoor, yCoor);
            }
            else
            {
                return SearchNewMaelstromPosition(X + 2, Y - 1, xCoor, yCoor, size, grid);
            }
        }
        else
        {
            return SearchNewMaelstromPosition(X, Y, xCoor, yCoor, size, grid);
        }
    }
    private (int, int) SearchNewMaelstromPosition(int MaelstromPositionX, int MaelstromPositionY, int xCoor, int yCoor, int size, ICell[,] grid)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int NewMaelstromPositionX = MaelstromPositionX + i;
                int NewMaelstromPositionY = MaelstromPositionY + j;
                if ((NewMaelstromPositionX < size && NewMaelstromPositionY >= 0) && grid[NewMaelstromPositionX, NewMaelstromPositionY].GetType() == typeof(Empty))
                {
                    X = NewMaelstromPositionX;
                    Y = NewMaelstromPositionY;
                    if (xCoor - 2 == -1)
                        xCoor = size - 1;
                    else if (xCoor - 2 == -2)
                        xCoor = size - 2;
                    else
                        xCoor = xCoor - 2;

                    if (yCoor + 1 == size)
                        yCoor = 0;
                    else
                        yCoor = yCoor + 1;
                    return (xCoor, yCoor);
                }
            }
        }
        return (xCoor, yCoor);
    }

}