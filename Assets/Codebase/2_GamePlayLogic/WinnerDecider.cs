using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinnerDecider : MonoBehaviour
{
    public class WinPlayer
    {
        string value;
        List<Card> cards;

        public WinPlayer(string value, List<Card> cards)
        {

        }
    }

    public void Decider()
    {
        // Create a list of players and their hands
        List<WinPlayer> players = new List<WinPlayer>()
        {
            new WinPlayer("Player 1", new List<Card>() { new Card(Suit.Clubs, Rank.Ace), new Card(Suit.Clubs, Rank.King) }),
            new WinPlayer("Player 2", new List<Card>() { new Card(Suit.Diamonds, Rank.Queen), new Card(Suit.Diamonds, Rank.Jack) }),
            new WinPlayer("Player 3", new List<Card>() { new Card(Suit.Spades, Rank.Ten), new Card(Suit.Hearts, Rank.Nine) }),
            new WinPlayer("Player 4", new List<Card>() { new Card(Suit.Clubs, Rank.Seven), new Card(Suit.Clubs, Rank.Six) }),
            new WinPlayer("Player 5", new List<Card>() { new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Diamonds, Rank.Ace) })
        };

        // Create the community cards
        List<Card> communityCards = new List<Card>() { new Card(Suit.Hearts, Rank.King), new Card(Suit.Diamonds, Rank.Ten), new Card(Suit.Clubs, Rank.Five), new Card(Suit.Hearts, Rank.Four), new Card(Suit.Diamonds, Rank.Two) };

        // Determine the winner(s)
        List<WinPlayer> winners = DetermineWinners(players, communityCards);

        // Print out the winner(s)
        if (winners.Count == 1)
        {
            // Console.WriteLine("The winner is " + winners[0].Name);
        }
        else
        {
            Console.Write("The winners are ");
            for (int i = 0; i < winners.Count; i++)
            {
                //Console.Write(winners[i].Name);
                if (i != winners.Count - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine();
        }
    }

    // Determines the winner(s) of a Texas Hold'em poker game
    public static List<WinPlayer> DetermineWinners(List<WinPlayer> players, List<Card> communityCards)
    {
        List<WinPlayer> winners = new List<WinPlayer>();
        int highestHandValue = 0;

        foreach (WinPlayer player in players)
        {
            // Combine the player's cards with the community cards to form a 7-card hand
            //List<Card> hand = player.Cards.Concat(communityCards).ToList();

            // Get the value of the player's hand
            // int handValue = GetHandValue(hand);

            // Check if this player's hand beats the highest hand value seen so far
            //if (handValue > highestHandValue)
            //{
            //    winners.Clear();
            //    winners.Add(player);
            //    highestHandValue = handValue;
            //}
            //else if (handValue == highestHandValue)
            //{
            //    // This player's hand ties with the highest hand value seen so far, so add them to the list of winners
            //    winners.Add(player);
            //}
        }

        return winners;
    }

    // Gets the value of a 5-card poker hand
    public static int GetHandValue(List<Card> hand)
    {
        // Sort the cards by rank (highest to lowest)
        // List<Card> sortedHand = hand.OrderByDescending(c => c.Rank).ToList();

        // remove it 
        return 0;
        // Check
    }
}
