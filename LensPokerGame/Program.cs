using System;

namespace LensPokerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            Hand[] hands = new Hand[4];

            //deck.ShuffleDeck();

            Hand str = new Hand();


            str.PrintHand();
            Console.WriteLine();

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i] = new Hand(deck);
                Console.WriteLine("Hand {0}", i + 1);
                hands[i].PrintHand();                
                Console.WriteLine();
            }

        }
    }
}
