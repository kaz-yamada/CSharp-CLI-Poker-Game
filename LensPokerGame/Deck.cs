using System;
using System.Collections.Generic;
using System.Text;

namespace LensPokerGame
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades };
    public enum FaceValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
    public enum HandType { HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush };

    class Deck
    {
        private const int Size = 52;
        private readonly Card[] Cards = new Card[Size];

        public Deck()
        {
            int index = 0;

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue face in Enum.GetValues(typeof(FaceValue)))
                {
                    Cards[index] = new Card(suit, face);
                    index++;
                }
            }
        }

        public void PrintDeck()
        {
            foreach (Card card in Cards)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public void ShuffleDeck()
        {
            Random random = new Random();
            int n = Size;
            while (n > 1)
            {
                int k = random.Next(n--);
                Card temp = Cards[n];
                Cards[n] = Cards[k];
                Cards[k] = temp;
            }
        }

        public Card DrawCard()
        {
            int i = Array.FindIndex(Cards, i => !i.IsDrawn);

            if (i >= 0)
            {
                Cards[i].IsDrawn = true;
                return Cards[i];
            }

            return Cards[0];
        }
    }

    class Card : IComparable<Card>
    {
        public Suit Suit { get; private set; }
        public FaceValue FaceValue { get; private set; }
        public bool IsDrawn { get; set; } = false;

        public Card(Suit suit, FaceValue faceValue)
        {
            this.Suit = suit;
            this.FaceValue = faceValue;
        }

        public override string ToString() => $"{FaceValue} of {Suit}";

        public int CompareTo(Card other)
        {
            if (FaceValue == other.FaceValue)
            {
                return Suit - other.Suit;
            }

            return FaceValue - other.FaceValue;
        }

    }

    class Hand
    {
        private const int HandSize = 5;
        private Card[] Cards = new Card[HandSize];
        private readonly HashSet<Suit> Suits = new HashSet<Suit>();
        private int HandType = 0;
        private Dictionary<FaceValue, int> pairs = new Dictionary<FaceValue, int>();

        private FaceValue HighValue;
        private Suit HighSuit;

        public static string[] HandTypeString = { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush", "Royal Flush" };

        public Hand(Deck deck)
        {
            for (int i = 0; i < HandSize; i++)
            {
                Cards[i] = deck.DrawCard();
                FaceValue val = Cards[i].FaceValue;

                if (pairs.ContainsKey(val))
                {
                    pairs[val]++;
                }
                else
                {
                    pairs[val] = 1;
                }

                Suits.Add(Cards[i].Suit);
            }
            Array.Sort(Cards);

            HandType = CheckHand();
        }

        public void PrintHand()
        {
            foreach (Card card in Cards)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public int CheckHand()
        {
            bool isStraight = false;

            for (int i = 0; i < HandSize; i++)
            {

            }

            // Flush
            if (Suits.Count == 1)
            {
                // Straight Flush
                return isStraight ? 8 : 5;
            }

            return 0;
        }



        public static bool operator >(Hand hand1, Hand hand2)
        {
            return hand1.CheckHand() > hand2.CheckHand();
        }

        public static bool operator <(Hand hand1, Hand hand2)
        {
            return hand1.CheckHand() < hand2.CheckHand();
        }
    }
}