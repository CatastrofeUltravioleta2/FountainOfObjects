public class Game
{
    public ICell[,] grid;
    public (int X, int Y) CurrentPosition = (0, 0);
    public int Arrows { get; set; } = 5;
    private string ShootingResult = "";
    private bool Help = false;
    public Entrance entrance;
    public Fountain fountain;
    public Pits[] pits;
    public Amaroks[] amaroks;
    public Maelstroms[] maelstroms;
    public Game(int size)
    {
        (grid, entrance, pits, amaroks, maelstroms, fountain) = RandomMap.RandomGenerator(size);
    }
    public string Sense()
    {
        string UserSense = "";
        UserSense += grid[CurrentPosition.X, CurrentPosition.Y].Description;
        if (CheckForType<Maelstroms>(CurrentPosition.X, CurrentPosition.Y))
        {
            foreach (Maelstroms maelstrom in maelstroms)
            {
                if (maelstrom.X == CurrentPosition.X && maelstrom.Y == CurrentPosition.Y)
                {
                    (int X, int Y) lastPosition = CurrentPosition;
                    CurrentPosition = maelstrom.Encounter(CurrentPosition.X, CurrentPosition.Y, grid.GetLength(0), grid);
                    grid[lastPosition.X, lastPosition.Y] = new Empty();
                    grid[maelstrom.X, maelstrom.Y] = maelstrom;
                }
            }
        }
        else
        {
            foreach (Pits pit in pits)
            {
                if (pit.IsNearby(CurrentPosition.X, CurrentPosition.Y))
                {
                    UserSense += "\n" + "You feel a draft. There is a pit in a nearby room";
                }
            }
            foreach (Amaroks amarok in amaroks)
            {
                if (amarok.IsNearby(CurrentPosition.X, CurrentPosition.Y) && amarok.IsAlive)
                {
                    UserSense += "\n" + "You can smell the rotten stench of an amarok in a nearby room";
                }
            }
            foreach (Maelstroms maelstrom in maelstroms)
            {
                if (maelstrom.IsNearby(CurrentPosition.X, CurrentPosition.Y) && maelstrom.IsAlive)
                {
                    UserSense += "\n" + "You hear the growling and groaning of a maelstrom nearby";
                }
            }
            if (ShootingResult != "")
            {
                UserSense += "\n" + ShootingResult;
                ShootingResult = "";
            }
            if (Arrows > 0)
            {
                UserSense += "\n" + $"You have {Arrows} arrows left";
            }
            else
            {
                UserSense += "\n" + $"Yon don't have arrows left, you cannot shoot";
            }
            if (Help)
            {
                UserSense += "\n" + "------------------------------------------------------------";
                UserSense += "\n" + "- move (north, south, east, west): moves the player to said direction";
                UserSense += "\n" + "- enable/disable fountain: actives or deactivates the fountain, only works in the fountain room";
                UserSense += "\n" + "- shoot (north, south, east, west): shoots one arrows to the room in that direction, it can kill Amaroks and Maelstorms";
                UserSense += "\n" + "- help: shows this menu";
                Help = false;
            }
        }
        return UserSense;
    }
    public void Action(string input)
    {
        switch (input)
        {
            case "move south":
                if (CurrentPosition.Y != 0)
                    CurrentPosition.Y -= 1;
                else
                    throw new NotSupportedException();
                break;
            case "move north":
                if (CurrentPosition.Y != grid.GetLength(1) - 1)
                    CurrentPosition.Y += 1;
                else
                    throw new NotSupportedException();
                break;
            case "move east":
                if (CurrentPosition.X != 0)
                    CurrentPosition.X -= 1;
                else
                    throw new NotSupportedException();
                break;
            case "move west":
                if (CurrentPosition.X != grid.GetLength(0) - 1)
                    CurrentPosition.X += 1;
                else
                    throw new NotSupportedException();
                break;
            case "enable fountain":
            case "disable fountain":
                if (grid[CurrentPosition.X, CurrentPosition.Y].GetType() == typeof(Fountain))
                {
                    Fountain myFountain = (Fountain)grid[CurrentPosition.X, CurrentPosition.Y];
                    myFountain.EnableDisable(input);
                }
                else
                {
                    throw new ArgumentException();
                }
                break;
            case "shoot west":
                Shoot("west");
                break;
            case "shoot east":
                Shoot("east");
                break;
            case "shoot north":
                Shoot("north");
                break;
            case "shoot south":
                Shoot("south");
                break;
            case "help":
                Help = true;
                break;
            case "show":
                Show();
                break;
            default:
                throw new NotImplementedException();
        }
    }
    public void Shoot(string input)
    {
        if (Arrows > 0)
        {
            switch (input)
            {
                case "north":
                    if (CurrentPosition.Y != grid.GetLength(0) - 1)
                    {
                        ShootCreature<Amaroks>(0, 1, amaroks);
                        ShootCreature<Maelstroms>(0, 1, maelstroms);
                        Arrows -= 1;
                    }
                    else
                    {
                        ShootingResult = "You cannot shoot in a wall";
                    }
                    break;
                case "south":
                    if (CurrentPosition.Y != 0)
                    {
                        ShootCreature<Amaroks>(0, -1, amaroks);
                        ShootCreature<Maelstroms>(0, -1, maelstroms);
                        Arrows -= 1;
                    }
                    else
                    {
                        ShootingResult = "You cannot shoot in a wall";
                    }
                    break;
                case "west":
                    if (CurrentPosition.X != grid.GetLength(0) - 1)
                    {
                        ShootCreature<Amaroks>(1, 0, amaroks);
                        ShootCreature<Maelstroms>(1, 0, maelstroms);
                        Arrows -= 1;
                    }
                    else
                    {
                        ShootingResult = "You cannot shoot in a wall";
                    }
                    break;
                case "east":
                    if (CurrentPosition.X != 0)
                    {
                        ShootCreature<Amaroks>(-1, 0, amaroks);
                        ShootCreature<Maelstroms>(-1, 0, maelstroms);
                        Arrows -= 1;
                    }
                    else
                    {
                        ShootingResult = "You cannot shoot in a wall";
                    }
                    break;
            }
        }
    }
    public bool IsWin()
    {
        return fountain.isEnabled == true && CurrentPosition == (0, 0);
    }
    public bool IsPlayerDead()
    {
        bool IsAmarokAlive = false;
        foreach (Amaroks amarok in amaroks)
        {
            if (amarok.X == CurrentPosition.X && amarok.Y == CurrentPosition.Y)
            {
                IsAmarokAlive = amarok.IsAlive;
            }

        }
        return CheckForType<Pits>(CurrentPosition.X, CurrentPosition.Y) || IsAmarokAlive;
    }
    public bool CheckForType<T>(int XCoordinate, int YCoordinate) where T : ICell
    {
        return grid[XCoordinate, YCoordinate].GetType() == typeof(T);
    }
    public bool DetectMaelstrom()
    {
        bool IsMaelstromAlive = false;
        foreach (Maelstroms maelstrom in maelstroms)
        {
            if (maelstrom.X == CurrentPosition.X && maelstrom.Y == CurrentPosition.Y)
            {
                IsMaelstromAlive = maelstrom.IsAlive;
            }
        }
        return IsMaelstromAlive;
    }
    public void ShootCreature<T>(int x, int y, T[] monsters) where T : Monsters
    {
        if (CheckForType<T>(CurrentPosition.X + x, CurrentPosition.Y + y))
        {
            foreach (T monster in monsters)
            {
                if (monster.X == CurrentPosition.X + x && monster.Y == CurrentPosition.Y + y)
                {
                    grid[monster.X, monster.Y] = new Empty();
                    monster.IsAlive = false;
                    ShootingResult = $"You just killed a {monster.ToString()}";
                    break;
                }
            }
        }
    }
    public void Show()
    {
        for(int j = grid.GetLength(0)-1;j>-1;j--)
        {
            for(int i = 0;i<grid.GetLength(1); i++)
            {
                if(grid[i,j].GetType() == typeof(Empty))
                {
                    Console.Write($"{"_",4}");
                }
                else if(grid[i,j].GetType() == typeof(Entrance))
                {
                    Console.Write($"{"E",4}");
                }
                else if(grid[i,j].GetType() == typeof(Fountain))
                {
                    Console.Write($"{"F",4}");
                }
                else if(grid[i,j].GetType() == typeof(Pits))
                {
                    Console.Write($"{"P",4}");
                }
                else if(grid[i,j].GetType() == typeof(Amaroks))
                {
                    Console.Write($"{"A",4}");
                }
                else if(grid[i,j].GetType() == typeof(Maelstroms))
                {
                    Console.Write($"{"M",4}");
                }
            }
            Console.WriteLine("\n");
        }
    }
}
