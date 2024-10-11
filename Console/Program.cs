Console.Clear();
Game game = null;
bool IsSizeSelected = false;
do
{
    Console.WriteLine("Select the size of the game, it's a grid of a*a (should be greater than 4)");
    if(int.TryParse(Console.ReadLine(), out int choice))
    {
        if(choice>=4)
        {
            game = new Game(choice);
            IsSizeSelected = true;
        }
    }
}
while (!IsSizeSelected);
Console.Clear();

Console.WriteLine("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits ins search of the Fountain of Objects");
Console.WriteLine("Light is visible only in the entrance, and no other light is seen anywhere in the caverns");
Console.WriteLine("You must navigate the Caverns with your other senses");
Console.WriteLine("Find the Fountain of Objects, activate it, and return to the entrance");
Console.WriteLine("Look out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will die");
Console.WriteLine("Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any location. You will be able to hear their growling in nearby room");
Console.WriteLine("Amaroks roam the caverns, Encountering one is certain death, but you can smell their rotten stench in nearby rooms");
Console.WriteLine("You carry with you a bow and a quiver of arrows. You can use them to shoot monsters in the caverns but be warned: you have a limited supply");
while (!game.IsWin())
{
    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine($"You are in the room at: (Column={game.CurrentPosition.X}, Row={game.CurrentPosition.Y})");
    if (!game.IsPlayerDead())
    {
        if (game.DetectMaelstrom())
        {
            Console.WriteLine(game.Sense());
        }
        else
        {
            Console.WriteLine(game.Sense());
            Console.Write("What do you want to do: ");
            try
            {
                game.Action(Console.ReadLine());
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("You are trying to go out of bounds");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Professor's words echoed: there's a time and place for everything but not know");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("That's not an action, try again");
            }
        }
    }
    else
    {
        Console.WriteLine(game.grid[game.CurrentPosition.X, game.CurrentPosition.Y].Description);
        break;
    }

}
if (game.IsWin())
{
    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine($"You are in the room at: (Column={game.CurrentPosition.X}, Row={game.CurrentPosition.Y}");
    Console.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life");
    Console.WriteLine("You Win");
}
else
{
    Console.WriteLine("You are dead, thanks for playing though");
}
