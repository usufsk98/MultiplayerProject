using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpritesHolder : Singleton_IndependentObject<SpritesHolder>
{
    [SerializeField]
    private List<SpriteCardInfoPair> _spriteCardInfoList;
    private Dictionary<CardInfo, Sprite> _spriteCardInfoDict;

    public override void Awake()
    {
        base.Awake();

        // Convert the list to a dictionary at runtime
        _spriteCardInfoDict = new Dictionary<CardInfo, Sprite>();
        foreach (SpriteCardInfoPair pair in _spriteCardInfoList)
        {
            _spriteCardInfoDict.Add(pair.key, pair.value);
        }
    }
    public void DoSomething()
    {
        //List<Card> cards = new List<Card>();
        //_spriteCardInfoList = new List<SpriteCardInfoPair>();

        //// create a deck of 52 cards
        //foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        //{
        //    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        //    {
        //        cards.Add(new Card(suit, rank));
        //        SpriteCardInfoPair infoPair = new SpriteCardInfoPair(null, new CardInfo(rank, suit));
        //        _spriteCardInfoList.Add(infoPair);
        //        // Debug.Log("Card Name " + cards[cards.Count - 1]);
        //    }
        //}
    }
    public Sprite HaveCard(Card card)
    {
        foreach (SpriteCardInfoPair pair in _spriteCardInfoList)
        {
            if (pair.key.suit == card.suit && pair.key.rank == card.rank)
                return pair.value;
        }
        throw new Exception("No Card Matching");
    }
}

[Serializable]
public class CardInfo
{
    public Rank rank;
    public Suit suit;

    public CardInfo(Rank rank, Suit suit)
    {
        this.rank = rank;
        this.suit = suit;
    }
}

[Serializable]
public class SpriteCardInfoPair
{
    public Sprite value;
    public CardInfo key;

    public SpriteCardInfoPair(Sprite value, CardInfo key)
    {
        this.key = key;
        this.value = value;
    }
}
