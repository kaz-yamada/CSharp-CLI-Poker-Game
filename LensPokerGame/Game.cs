using System;
using System.Collections.Generic;

namespace LensPokerGame
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades };
    public enum FaceValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
    public enum HandType { HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush };

    class Game
    {
        private const int Size = 52;
        private readonly Card[] Cards = new Card[Size];
        private Hand[] Hands;
        private int PlayerCount;
        private int WinnerIndex;

        public Game(int count = 4)
        {
            int index = 0;
            PlayerCount = count;
            Hands = new Hand[4];
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

        public void StartGame()
        {
            ShuffleDeck();

            for (int i = 0; i < PlayerCount; i++)
            {
                Hands[i] = new Hand(this);
                Console.WriteLine("Hand {0}", i + 1);
                Hands[i].PrintHand();
                Console.WriteLine();

                if (WinnerIndex < 0 || Hands[i] > Hands[WinnerIndex])
                {
                    WinnerIndex = i;
                }
            }
        }

        public void PrintWinner()
        {
            Console.WriteLine("-------------------\nWinning Hand number {0}:\n", WinnerIndex + 1);
            Hands[WinnerIndex].PrintHand();
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


        public static bool operator >(Card card1, Card card2)
        {
            if (card1.FaceValue == card2.FaceValue)
            {
                return card1.Suit > card2.Suit;
            }

            return card1.FaceValue > card2.FaceValue;
        }

        public static bool operator <(Card card1, Card card2)
        {
            if (card1.FaceValue == card2.FaceValue)
            {
                return card1.Suit < card2.Suit;
            }

            return card1.FaceValue < card2.FaceValue;
        }
    }

    class Hand
    {
        private const int HandSize = 5;
        private readonly Card[] Cards = new Card[HandSize];
        private readonly HashSet<Suit> Suits = new HashSet<Suit>();
        private readonly Dictionary<FaceValue, int> pairs = new Dictionary<FaceValue, int>();
        private readonly bool HasPair = false;
        public HandType HandType { get; private set; } = 0;
        public FaceValue HighValue { get; private set; }

        public static string[] HandTypeString = { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush", "Royal Flush" };

        public Hand(Game deck)
        {
            for (int i = 0; i < HandSize; i++)
            {
                Cards[i] = deck.DrawCard();
                FaceValue val = Cards[i].FaceValue;

                if (pairs.ContainsKey(val))
                {
                    pairs[val]++;
                    HasPair = true;
                }
                else
                {
                    pairs[val] = 1;
                }

                Suits.Add(Cards[i].Suit);
            }
            Array.Sort(Cards);
            CheckHandType();
        }

        public void PrintHand()
        {
            foreach (Card card in Cards)
            {
                Console.WriteLine(card.ToString());
            }

            Console.WriteLine("{0} - {1}", HandTypeString[(int)HandType], HighValue);
        }


        /// <summary>
        /// Checks if there is a straight in the hand 
        /// </summary>
        /// <returns></returns>
        private bool IsStraight()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    // Checks if the hand is Ace high or low, straight cannot be high or low e.g QKA23 is an ace high hand
                    if (Cards[i].FaceValue == FaceValue.Five)
                    {
                        return Cards[i + 1].FaceValue == FaceValue.Ace || Cards[i + 1].FaceValue == FaceValue.Six;
                    }
                    else if (Cards[i].FaceValue == FaceValue.King)
                    {
                        return Cards[i + 1].FaceValue == FaceValue.Ace;
                    }
                }


                if ((int)Cards[i].FaceValue + 1 != (int)Cards[i + 1].FaceValue)
                {
                    return false;
                }
            }

            return true;
        }

        private void FindPairs()
        {
            // Check for pairs
            foreach (KeyValuePair<FaceValue, int> pair in pairs)
            {
                if (pair.Value > 1 && pair.Key > HighValue)
                {
                    HighValue = pair.Key;
                }

                if (pair.Value == 4)
                {
                    HandType = HandType.FourOfAKind;
                    return;
                }

                if (HandType == HandType.HighCard)
                {
                    // One Pair
                    if (pair.Value == 2)
                    {
                        HandType = HandType.OnePair;
                    }

                    // Three of a Kind
                    if (pair.Value == 3)
                    {
                        HandType = HandType.ThreeOfAKind;
                    }
                }
                else if (HandType == HandType.OnePair)
                {
                    // Two Pair
                    if (pair.Value == 2)
                    {
                        HandType = HandType.TwoPair;
                    }

                    // Full house
                    if (pair.Value == 3)
                    {
                        HandType = HandType.FullHouse;
                    }
                }
                else if (HandType == HandType.ThreeOfAKind && pair.Value == 3)
                {
                    HandType = HandType.FullHouse;
                }
            }

        }

        public void CheckHandType()
        {
            bool HasStraight = IsStraight();

            if (HasStraight)
            {
                HandType = HandType.Straight;
            }
            else if (HasPair)
            {
                FindPairs();
            }

            // Hand is a flush
            if (Suits.Count == 1)
            {
                // Check for royal straight flush
                if (HasStraight)
                {
                    if (Cards[0].FaceValue == FaceValue.Ten && Cards[4].FaceValue == FaceValue.Ace)
                    {
                        HandType = HandType.RoyalFlush;
                    }
                    HandType = HandType.StraightFlush;
                }
            }

            if (HandType == HandType.HighCard)
            {
                HighValue = Cards[4].FaceValue;
            }
        }

        public static bool operator >(Hand hand1, Hand hand2)
        {
            if (hand1.HandType == hand2.HandType)
            {
                return hand1.HighValue > hand2.HighValue;
            }

            return hand1.HandType > hand2.HandType;
        }
        public static bool operator <(Hand hand1, Hand hand2)
        {
            if (hand1.HandType == hand2.HandType)
            {
                return hand1.HighValue < hand2.HighValue;
            }

            return hand1.HandType < hand2.HandType;
        }
    }
}