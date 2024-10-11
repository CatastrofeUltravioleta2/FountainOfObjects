using System.Drawing;
using System.Runtime.CompilerServices;

public class RandomMap
{

    public static (ICell[,], Entrance, Pits[], Amaroks[], Maelstroms[], Fountain) RandomGenerator(int size)
    {

        if (size >= 4)
        {
            Random random = new Random();
            string Path = "";
            (int xFountain, int yFountain) = SelectFountain(size);
            for (int i = 0; i < xFountain; i++)
                Path += 0;
            for (int i = 0; i < yFountain; i++)
                Path += 1;
            string randomPath = ScrambleWord(Path);
            (int, int)[] ClearCells = ClearPath(randomPath);

            ICell[,] grid = new ICell[size, size];
            foreach ((int, int) cell in ClearCells)
            {
                grid[cell.Item1, cell.Item2] = new Empty();
            }
            Entrance entrance = new Entrance();
            grid[0, 0] = entrance;
            Fountain fountain = new Fountain(xFountain, yFountain);
            grid[fountain.X, fountain.Y] = fountain;
            Console.WriteLine("Fountain");

            Pits[] newPits = new Pits[size - 2];
            Amaroks[] newAmaroks = new Amaroks[size - 2];
            Maelstroms[] newMaelstroms = new Maelstroms[(int)Math.Floor(Math.Sqrt(size-1))];

            for (int i = 0; i < newPits.Length; i++)
            {
                while (true)
                {
                    int newPitX = random.Next(0, size);
                    int newPitY = random.Next(0, size);
                    if (grid[newPitX, newPitY] == null)
                    {
                        newPits[i] = new Pits(newPitX, newPitY);
                        grid[newPitX,newPitY] = newPits[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < newAmaroks.Length; i++)
            {
                while (true)
                {
                    int newAmarokX = random.Next(0, size);
                    int newAmarowY = random.Next(0, size);
                    if (grid[newAmarokX, newAmarowY] == null)
                    {
                        newAmaroks[i] = new Amaroks(newAmarokX, newAmarowY);
                        grid[newAmarokX,newAmarowY] = newAmaroks[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < newMaelstroms.Length; i++)
            {
                Console.WriteLine(i);
                while (true)
                {
                    int newMaelstromX = random.Next(0, size);
                    int newMaelstromY = random.Next(0, size);
                    if (grid[newMaelstromX, newMaelstromY] == null)
                    {
                        newMaelstroms[i] = new Maelstroms(newMaelstromX, newMaelstromY);
                        grid[newMaelstromX,newMaelstromY] = newMaelstroms[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == null)
                    {
                        grid[i, j] = new Empty();
                    }
                }
            }
            return (grid, entrance, newPits, newAmaroks, newMaelstroms, fountain);
        }
        else
        {
            throw new Exception();
        }
    }
    private static string ScrambleWord(string word)
    {
        char[] chars = new char[word.Length];
        Random rand = new Random(10000);
        int index = 0;
        while (word.Length > 0)
        {
            int next = rand.Next(0, word.Length - 1);

            chars[index] = word[next];
            word = word.Substring(0, next) + word.Substring(next + 1);
            ++index;
        }
        return new String(chars);
    }
    private static (int, int)[] ClearPath(string path)
    {
        int x = 0;
        int y = 0;
        (int, int)[] clearCells = new (int, int)[path.Length - 1];
        for (int i = 0; i < path.Length - 1; i++)
        {
            if (path[i] == '0')
            {
                x += 1;
                clearCells[i] = (x, y);
            }
            else if (path[i] == '1')
            {
                y += 1;
                clearCells[i] = (x, y);
            }
        }
        return clearCells;
    }
    private static (int, int) SelectFountain(int size)
    {
        Random random = new Random();
        while (true)
        {
            int NewxFountain = random.Next(2, size);
            int NewyFountain = random.Next(2, size);
            if (NewxFountain < size - 2 && NewyFountain >= size - 2)
            {
                return (NewxFountain, NewyFountain);
            }
            else if (NewyFountain < size - 2 && NewxFountain >= size - 2)
            {
                return (NewxFountain, NewyFountain);
            }
            else if (NewyFountain >= size - 2 && NewxFountain >= size - 2)
            {
                return (NewxFountain, NewyFountain);
            }
        }
    }
}