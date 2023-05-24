using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public enum Rank
{
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}


[Serializable]
public class Card
{
    public Suit suit;
    public Rank rank;

    public Card(Suit suit, Rank rank)
    {
        this.suit = suit;
        this.rank = rank;
    }

    public override string ToString()
    {
        string myRank = " ";
        
        switch (rank)
        {
            case Rank.Ace:
                myRank = "A";
                break;
            case Rank.Two:
            case Rank.Three:
            case Rank.Four:
            case Rank.Five:
            case Rank.Six:
            case Rank.Seven:
            case Rank.Eight:
            case Rank.Nine:
            case Rank.Ten:
                myRank = ((int)rank).ToString();
                break;
            case Rank.Jack:
                myRank = "J";
                break;
            case Rank.Queen:
                myRank = "Q";
                break;
            case Rank.King:
                myRank = "K";
                break;
        }
        return (myRank);
    }
}

[Serializable]
public class Deck
{
    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();

        // create a deck of 52 cards
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
               // Debug.Log("Card Name " + cards[cards.Count - 1]);
            }
        }
    }

    public void Shuffle()
    {
        // shuffle the deck of cards using the Fisher-Yates shuffle algorithm
        for (int i = cards.Count - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public Card DrawCard()
    {
        // remove and return the top card from the deck
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public int Count()
    {
        return cards.Count;
    }

    public void Clear()
    {
        cards.Clear();
    }
}
