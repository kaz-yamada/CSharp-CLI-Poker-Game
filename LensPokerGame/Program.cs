using System;

namespace LensPokerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
            game.PrintWinner();
        }
    }
}
