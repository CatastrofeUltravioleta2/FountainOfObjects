public abstract class Obstacles : SingleElements
{
    public Obstacles(int x, int y) : base(x, y)
    {
    }

    virtual public bool IsNearby(int xCoor, int yCoor)
    {
        if ((X == xCoor - 1 || X == xCoor + 1) && (Y == yCoor - 1 || Y == yCoor + 1))
        {
            return true;
        }
        else if ((X == xCoor && (Y == yCoor - 1 || Y == yCoor + 1)) || (Y == yCoor && (X == xCoor - 1 || X == xCoor + 1)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class Pits : Obstacles
{
    public Pits(int x, int y) : base(x, y)
    {
        Description = "You fell into a pit, you are still faliing, yup no coming back";
    }

}

