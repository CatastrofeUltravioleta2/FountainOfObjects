public interface ICell
{
    public string Description { get; set; }
}

public class Entrance : ICell
{
    public string Description { get; set; } = "You can sense light coming from the outside";
}

public class Empty : ICell
{
    public string Description { get; set; } = "There is nothing here, keep searching";
}

