public abstract class SingleElements : ICell
{
    public int X { get; set; }
    public int Y { get; set; }

    public string Description { get; set; } = "";

    public SingleElements(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Fountain : SingleElements
{
    public bool isEnabled = false;
    public Fountain(int x, int y) : base(x, y)
    {
        Description = "You hear water dripping in this room. The fountain of Objects is here";
    }

    public void EnableDisable(string input)
    {
        if (isEnabled == false && input == "enable fountain")
        {
            isEnabled = true;
            Description = "You hear the rushing waters from the Fountain of Objects. It has been reactivated";
        }
        else if (isEnabled == true && input == "disable fountain")
        {
            isEnabled = false;
            Description = "The fountain of Objects has been disabled, YOU ARE SUPPOSED TO ENABLE IT TO WIN";
        }
    }
}
