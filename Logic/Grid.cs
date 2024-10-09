public class Game
{
    public ICell[,] grid;
    public (int X, int Y) CurrentPosition = (0, 0);
    public int Arrows { get; set; } = 5;
    private string ShootingResult = "";
    private bool Help = false;
    public Fountain fountain;
    public Pits[] pits;
    public Amaroks[] amaroks;
    public Maelstroms[] maelstroms;
    public Game(int size)
    {
        if (size == 4)
        {
            grid = new ICell[4, 4];
            pits = new Pits[] { new Pits(0, 2), new Pits(2, 0) };
            amaroks = new Amaroks[] { new Amaroks(1, 2), new Amaroks(3, 3) };
            maelstroms = new Maelstroms[] { new Maelstroms(0, 1) };
            fountain = new Fountain(3, 2);

        }
        else if (size == 6)
        {
            grid = new ICell[6, 6];
            pits = new Pits[] { new Pits(2, 0), new Pits(1, 5), new Pits(4, 1), new Pits(4, 4) };
            amaroks = new Amaroks[] { new Amaroks(0, 4), new Amaroks(1, 2), new Amaroks(2, 2), new Amaroks(5, 2) };
            maelstroms = new Maelstroms[] { new Maelstroms(3, 3) };
            fountain = new Fountain(2, 5);

        }
        else if (size == 8)
        {
            grid = new ICell[8, 8];
            pits = new Pits[] { new Pits(0, 6), new Pits(1, 4), new Pits(4, 1), new Pits(4, 5), new Pits(7, 0), new Pits(7, 4) };
            amaroks = new Amaroks[] { new Amaroks(3, 0), new Amaroks(3, 6), new Amaroks(5, 0), new Amaroks(6, 1), new Amaroks(6, 5) };
            maelstroms = new Maelstroms[] { new Maelstroms(2, 3), new Maelstroms(6, 4) };
            fountain = new Fountain(7, 2);
        }
        grid![0, 0] = new Entrance();
        grid[fountain!.X, fountain.Y] = fountain;
        for (int i = 0; i < pits!.Length; i++)
        {
            grid[pits[i].X, pits[i].Y] = pits[i];
        }
        for (int i = 0; i < amaroks!.Length; i++)
        {
            grid[amaroks[i].X, amaroks[i].Y] = amaroks[i];
        }
        for (int i = 0; i < maelstroms!.Length; i++)
        {
            grid[maelstroms[i].X, maelstroms[i].Y] = maelstroms[i];
        }
        for (int i = 0; i < grid!.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)

            {
                if (grid[i, j] == null)
                {
                    grid[i, j] = new Empty();
                }
            }
        }
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
}
