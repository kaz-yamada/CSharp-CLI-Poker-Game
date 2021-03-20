using System;

namespace LensPokerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            Hand[] hands = new Hand[4];
            int winner = -1;            

            deck.ShuffleDeck();

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i] = new Hand(deck);
                Console.WriteLine("Hand {0}", i + 1);
                hands[i].PrintHand();
                Console.WriteLine();

                if (winner < 0 || hands[i] > hands[winner])
                {
                    winner = i;
                }
            }

            Console.WriteLine("-------------------\nWinning Hand number {0}:\n", winner + 1);
            hands[winner].PrintHand();
        }
    }
}
