// launch file
using Tetris;


namespace Compile
{
    class Handler
    {
        static void Main()
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.Title = "MAT";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("░▀▀█▀▀░▒█▀▀▀░▀▀█▀▀░▒█▀▀▄░▀█▀░▒█▀▀▀█░░░▄▀░▒█▀▄▀█░█▀▀▄░▀▀█▀▀░▀▄\n░░▒█░░░▒█▀▀▀░░▒█░░░▒█▄▄▀░▒█░░░▀▀▀▄▄░░░█░░▒█▒█▒█▒█▄▄█░░▒█░░░░█\n░░▒█░░░▒█▄▄▄░░▒█░░░▒█░▒█░▄█▄░▒█▄▄▄█░░░▀▄░▒█░░▒█▒█░▒█░░▒█░░░▄▀\n");
            
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HELP: up-arrow/W - rotate block; left-arrow/A - move left; down-arrow/S - move down faster; right-arrow/D - move right;\n      SPACE - fall down; ENTER (HOLD) - save block; ESC - close game;");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Game.Launch();
        }
    }
}