using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinningScenerio:MonoBehaviour 
{
    #region Test Variables
    [SerializeField] List<PlayerBoy> playersList;
    [SerializeField] List<Card> communityCardsList;
    #endregion
    [SerializeField] List<Card> cardsSet;
    [SerializeField] List<Card> sameSuitComunityCards;
    [SerializeField] bool royalFlush = false;
    [SerializeField] List<Winner> winners;
    [SerializeField] List<int> sequence;
    [SerializeField] int winnerIndex = 0;
    public enum WinnerHand 
    { 
        None,
        Royal_Flush, 
        Straight_Flush, 
        Four_Of_A_Kind, 
        Full_House,
        Flush,
        Straight,
        Three_Of_A_Kind,
        Two_Pair,Pair,
        High_Card 
    }

    public enum RoyalFlushRank
    {
        Ace = 1,
        Ten,
        Jack,
        Queen,
        King
    }

    [SerializeField] WinnerHand winnerHand;
    private void Start()
    {
        //TestScenario();
    }

   
    public PlayerBoy GetWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        winnerHand = WinnerHand.None;
        //DO DONT CHANGE THE FUNCTION CALLING ORDER  HERE 
        CheckRoyalFlushWinner(playerBoys, communityCards);
        CheckStraightFlushWinner(playerBoys, communityCards);
        CheckFourOfKindWinner(playerBoys, communityCards);
        CheckFullHouseWinner(playerBoys, communityCards);
        CheckFlushWinner(playerBoys, communityCards);
        CheckStraightWinner(playerBoys, communityCards);
        CheckThreeOfKindWinner(playerBoys, communityCards);
        CheckTwoPairWinner(playerBoys, communityCards);
        CheckPairWinner(playerBoys, communityCards);
        CheckHighCardWinner(playerBoys, communityCards);
        winnerHand = WinnerHand.None;
        return playerBoys[winnerIndex];
    }
    #region ROYAL_FLUSH
    void CheckRoyalFlushWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        bool royalFlush = false;
        winnerIndex = 0;
        for (int i = 0; i < playerBoys.Count; i++)
        {
            royalFlush = IsRoyalFlush(playerBoys[i].playerCards, communityCards);
            winnerIndex = i;
            if (royalFlush)
            {
                winnerHand = WinnerHand.Royal_Flush;
                //Debug.Log("PLayer " + winnerIndex + " is the Winner with Royal Flush");
                break;
            }
            else
            {
                //Debug.Log("NOT A ROYAL FLUSH");
            }
        }
    }
    bool IsRoyalFlush(List<Card> playerCards, List<Card> communityCards)
    {
        royalFlush = false;
        bool samesuit = false;
        for (int i = 1; i < playerCards.Count; i++)
        {
            if (playerCards[i].suit != playerCards[i - 1].suit)
            {
                samesuit = false;
                break;
            }
            else
                samesuit = true;
        }
        if (samesuit)
        {
            cardsSet = new List<Card>();
            sameSuitComunityCards = new List<Card>();
            int setCount = 0;

            for (int i = 0; i < communityCards.Count; i++)
            {
                if (communityCards[i].suit.Equals(playerCards[0].suit))
                    sameSuitComunityCards.Add(communityCards[i]);
            }

            if (sameSuitComunityCards.Count > 2)
            {
                Combinations(sameSuitComunityCards.Count);
                bool found = false;
                for (int i = 0; i < combinationList.Count; i++)
                {
                    cardsSet.Clear();
                    setCount = 0;
                    for (int k = 0; k < playerCards.Count; k++)
                    {
                        cardsSet.Add(playerCards[k]);
                        setCount = k;
                    }
                    string values = combinationList[i];
                    var query = from val in values.Split(',')
                                select int.Parse(val);
                    Debug.LogError(values);
                    foreach (int num in query)
                    {
                        Debug.Log(num);
                        cardsSet.Add(sameSuitComunityCards[num]);
                    }

                    cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
                    int count = 0;
                    foreach (string name in Enum.GetNames(typeof(RoyalFlushRank)))
                    {
                        if (!cardsSet[count].rank.ToString().Equals(name))
                        {
                            found = false;
                            break;
                        }
                        else
                        {
                            found = true;
                        }
                        count++;
                    }
                    if (found)
                    {
                        royalFlush = true;
                        break;
                    }
                }
            }
            else royalFlush = false;


        }
        else royalFlush = false;
        if(!royalFlush)
            winnerHand = WinnerHand.None;
        return royalFlush;
    }
    #endregion

    #region STRAIGHT_FLUSH
    void CheckStraightFlushWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;
        for (int i = 0; i < playerBoys.Count; i++)
        {
            Debug.Log("CHECK PLAYER");
            if (playerBoys[i].playerCards[0].suit.Equals(playerBoys[i].playerCards[1].suit))
            {
                Debug.Log("same PLAYER suit");

                winners.Add(IsStraightFlush(playerBoys[i].playerCards, communityCards, i));
                //Debug.Log("PLayer " + winnerIndex + " is the Winner with straight Flush");
            }
        }
        int hightest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[winners[i].cardSet.Count-1].rank > hightest)
                {
                    hightest = (int)winners[i].cardSet[winners[i].cardSet.Count-1].rank;
                    winnerIndex = winners[i].playerIndex;
                } 
            }
            
        }
    }
    Winner IsStraightFlush(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        Winner win = new Winner();
            cardsSet = new List<Card>();
            sameSuitComunityCards = new List<Card>();
            int setCount = 0;

            for (int i = 0; i < communityCards.Count; i++)
            {
                if (communityCards[i].suit.Equals(playerCards[0].suit))
                    sameSuitComunityCards.Add(communityCards[i]);
            }
        if (sameSuitComunityCards.Count > 2)
        {
            Combinations(sameSuitComunityCards.Count);
            bool isInSequence = false;
            for (int i = 0; i < combinationList.Count; i++)
            {
                cardsSet.Clear();
                setCount = 0;
                for (int k = 0; k < playerCards.Count; k++)
                {
                    cardsSet.Add(playerCards[k]);
                    setCount = k;
                }
                string values = combinationList[i];
                Debug.Log("Va;lue: " + values);
                var query = from val in values.Split(',')
                            select int.Parse(val);
                foreach (int num in query)
                {
                    cardsSet.Add(sameSuitComunityCards[num]);
                }

                cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
                sequence = new List<int>();
                for (int f = 0; f < cardsSet.Count; f++)
                {
                    sequence.Add((int)cardsSet[f].rank);
                    Debug.LogError(sequence[f]);
                }
                int seq = sequence[0];
                for (int m = 1; m < sequence.Count; m++)
                {
                    if ((sequence[m] - 1).Equals(seq))
                    {
                        isInSequence = true;
                        seq = sequence[m];
                    }
                    else
                    {
                        isInSequence = false;
                        break;
                    }
                }

                if (isInSequence)
                    break;
            }

            if (!isInSequence)
                winnerHand = WinnerHand.None;
            else
            {
                winnerHand = WinnerHand.Straight_Flush;
                for (int i = 0; i < cardsSet.Count; i++)
                {
                    Debug.Log(cardsSet[i]);
                    win.cardSet.Add(cardsSet[i]);
                }
                win.playerIndex = playerIndex;
                return win;
            }
        }
        return null;
    }
    #endregion

    #region FOUR_OF_A_KIND
    void CheckFourOfKindWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;
       
        for (int i = 0; i < playerBoys.Count; i++)
        {
            if(playerBoys[i].playerCards[0].rank.Equals(playerBoys[i].playerCards[1].rank))
            {
                winners.Add(IsFourOfKind(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if(winners[i]!=null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    Winner IsFourOfKind(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        int count = 0;
        for (int i = 0; i < communityCards.Count; i++)
        {
            if (playerCards[0].rank.Equals(communityCards[i].rank))
            {
                count++;
            }
        }
        if(count==2)
        {
            winnerHand = WinnerHand.Four_Of_A_Kind;
            Winner win = new Winner();
            win.playerIndex = playerIndex;
            win.cardSet = playerCards;//As all cards are same in this case
            return win;
        }
        else
        {
            winnerHand = WinnerHand.None;
        }
        return null;
    }
    #endregion

    #region FULL_HOUSE 
    void CheckFullHouseWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;

        for (int i = 0; i < playerBoys.Count; i++)
        {
            //if (playerBoys[i].playerCards[0].rank.Equals(playerBoys[i].playerCards[1].rank))
            {
                winners.Add(IsFullHouse(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    Winner IsFullHouse(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        Winner win = new Winner();
        cardsSet = new List<Card>();
        bool fullHouse = false;

        Combinations(communityCards.Count);
        for (int i = 0; i < combinationList.Count; i++)
        {
            cardsSet.Clear();
            for (int k = 0; k < playerCards.Count; k++)
            {
                cardsSet.Add(playerCards[k]);
            }
            string values = combinationList[i];
            var query = from val in values.Split(',')
                        select int.Parse(val);
            foreach (int num in query)
            {
                cardsSet.Add(communityCards[num]);
            }

            cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
            if (cardsSet[0].rank.Equals(cardsSet[2].rank) && cardsSet[3].rank.Equals(cardsSet[4].rank))
            {
                fullHouse = true;
                break;
            }
            else if (cardsSet[0].rank.Equals(cardsSet[1].rank) && cardsSet[2].rank.Equals(cardsSet[4].rank))
            {
                List<Card> temp = new List<Card>();
                for (int j = 2; j < cardsSet.Count; j++)
                {
                    temp.Add(cardsSet[j]);
                }
                temp.Add(cardsSet[0]);
                temp.Add(cardsSet[1]);
                cardsSet.Clear();
                cardsSet = temp;
                fullHouse = true;
                break;
            }
        }


        if (!fullHouse)
            winnerHand = WinnerHand.None;
        else
        {
            winnerHand = WinnerHand.Full_House;
            for (int i = 0; i < cardsSet.Count; i++)
            {
                Debug.Log(cardsSet[i]);
                win.cardSet.Add(cardsSet[i]);
            }
            win.playerIndex = playerIndex;
            return win;
        }

        return null;
    }
    #endregion

    #region FLUSH 
    void CheckFlushWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;
        for (int i = 0; i < playerBoys.Count; i++)
        {
            if (playerBoys[i].playerCards[0].suit.Equals(playerBoys[i].playerCards[1].suit))
            {
                winners.Add(IsFlush(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int hightest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[winners[i].cardSet.Count - 1].rank > hightest)
                {
                    hightest = (int)winners[i].cardSet[winners[i].cardSet.Count - 1].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }

        }
    }
    Winner IsFlush(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        Winner win = new Winner();
        cardsSet = new List<Card>();
        sameSuitComunityCards = new List<Card>();
        int setCount = 0;

        for (int i = 0; i < communityCards.Count; i++)
        {
            if (communityCards[i].suit.Equals(playerCards[0].suit))
            {
                sameSuitComunityCards.Add(communityCards[i]);
            }
        }

        if (sameSuitComunityCards.Count > 2)
        {
            Combinations(sameSuitComunityCards.Count);
            for (int i = 0; i < combinationList.Count; i++)
            {
                cardsSet.Clear();
                setCount = 0;
                for (int k = 0; k < playerCards.Count; k++)
                {
                    cardsSet.Add(playerCards[k]);
                    setCount = k;
                }
                string values = combinationList[i];
                Debug.Log("Va;lue: " + values);
                var query = from val in values.Split(',')
                            select int.Parse(val);
                foreach (int num in query)
                {
                    cardsSet.Add(sameSuitComunityCards[num]);
                }

                cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
            }

            if (cardsSet.Count < 5)
                winnerHand = WinnerHand.None;
            else
            {
                winnerHand = WinnerHand.Flush;
                for (int i = 0; i < cardsSet.Count; i++)
                {
                    Debug.Log(cardsSet[i]);
                    win.cardSet.Add(cardsSet[i]);
                }
                win.playerIndex = playerIndex;
                return win;
            }
        }
        return null;
    }
    #endregion

    #region STRAIGHT 
    void CheckStraightWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;
        for (int i = 0; i < playerBoys.Count; i++)
        {
            //if (playerBoys[i].playerCards[0].suit.Equals(playerBoys[i].playerCards[1].suit))
            {
                winners.Add(IsStraight(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int hightest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[winners[i].cardSet.Count - 1].rank > hightest)
                {
                    hightest = (int)winners[i].cardSet[winners[i].cardSet.Count - 1].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }

        }
    }
    Winner IsStraight(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        Winner win = new Winner();
        cardsSet = new List<Card>();
        sameSuitComunityCards = new List<Card>();
        int setCount = 0;

        for (int i = 0; i < communityCards.Count; i++)
        {
            //if (communityCards[i].suit.Equals(playerCards[0].suit))
                sameSuitComunityCards.Add(communityCards[i]);
        }
        if (sameSuitComunityCards.Count > 2)
        {
            Combinations(sameSuitComunityCards.Count);
            bool isInSequence = false;
            for (int i = 0; i < combinationList.Count; i++)
            {
                cardsSet.Clear();
                setCount = 0;
                for (int k = 0; k < playerCards.Count; k++)
                {
                    cardsSet.Add(playerCards[k]);
                    setCount = k;
                }
                string values = combinationList[i];
                var query = from val in values.Split(',')
                            select int.Parse(val);
                foreach (int num in query)
                {
                    cardsSet.Add(sameSuitComunityCards[num]);
                }

                cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
                sequence = new List<int>();
                for (int f = 0; f < cardsSet.Count; f++)
                {
                    sequence.Add((int)cardsSet[f].rank);
                }
                int seq = sequence[0];
                for (int m = 1; m < sequence.Count; m++)
                {
                    if ((sequence[m] - 1).Equals(seq))
                    {
                        isInSequence = true;
                        seq = sequence[m];
                    }
                    else
                    {
                        isInSequence = false;
                        break;
                    }
                }

                if (isInSequence)
                    break;
            }

            if (!isInSequence)
                winnerHand = WinnerHand.None;
            else
            {
                winnerHand = WinnerHand.Straight;
                for (int i = 0; i < cardsSet.Count; i++)
                {
                    Debug.Log(cardsSet[i]);
                    win.cardSet.Add(cardsSet[i]);
                }
                win.playerIndex = playerIndex;
                return win;
            }
        }
        return null;
    }
    #endregion

    #region THREE_OF_A_KIND 
    void CheckThreeOfKindWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;

        for (int i = 0; i < playerBoys.Count; i++)
        {
            if (playerBoys[i].playerCards[0].rank.Equals(playerBoys[i].playerCards[1].rank))
            {
                winners.Add(IsThreeOfKind(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    Winner IsThreeOfKind(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        int count = 0;
        for (int i = 0; i < communityCards.Count; i++)
        {
            if (playerCards[0].rank.Equals(communityCards[i].rank))
            {
                count++;
            }
        }
        if (count == 1)
        {
            winnerHand = WinnerHand.Three_Of_A_Kind;
            Winner win = new Winner();
            win.playerIndex = playerIndex;
            win.cardSet = playerCards;//As all cards are same in this case
            return win;
        }
        else
        {
            winnerHand = WinnerHand.None;
        }
        return null;
    }

    #endregion

    #region TWO_PAIR 
    void CheckTwoPairWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;

        for (int i = 0; i < playerBoys.Count; i++)
        {
            //if (playerBoys[i].playerCards[0].rank.Equals(playerBoys[i].playerCards[1].rank))
            {
                winners.Add(IsTwoPair(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    Winner IsTwoPair(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        cardsSet.Clear();
        int count = 0;
        for (int i = 0; i < communityCards.Count; i++)
        {
            for (int j = 0; j < playerCards.Count; j++)
            {
                if (playerCards[j].rank.Equals(communityCards[i].rank))
                {
                    cardsSet.Add(communityCards[i]);
                    count++;
                }
            }
            
        }
        if (count == 2)
        {
            cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
            winnerHand = WinnerHand.Two_Pair;
            Winner win = new Winner();
            win.playerIndex = playerIndex;
            win.cardSet = playerCards;
            return win;
        }
        else
        {
            winnerHand = WinnerHand.None;
        }
        return null;
    }
    #endregion

    #region PAIR 
    void CheckPairWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;

        for (int i = 0; i < playerBoys.Count; i++)
        {
            //if (playerBoys[i].playerCards[0].rank.Equals(playerBoys[i].playerCards[1].rank))
            {
                winners.Add(IsPair(playerBoys[i].playerCards, communityCards, i));
            }
        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    Winner IsPair(List<Card> playerCards, List<Card> communityCards, int playerIndex)
    {
        cardsSet.Clear();
        int count = 0;
        for (int i = 0; i < communityCards.Count; i++)
        {
            for (int j = 0; j < playerCards.Count; j++)
            {
                if (playerCards[j].rank.Equals(communityCards[i].rank))
                {
                    cardsSet.Add(communityCards[i]);
                    count++;
                }
            }

        }
        if (count == 1)
        {
            cardsSet = cardsSet.OrderBy(d => d.rank).ToList();
            winnerHand = WinnerHand.Pair;
            Winner win = new Winner();
            win.playerIndex = playerIndex;
            win.cardSet = playerCards;
            return win;
        }
        else
        {
            winnerHand = WinnerHand.None;
        }
        return null;
    }
    #endregion

    #region HIGH_CARD 
    void CheckHighCardWinner(List<PlayerBoy> playerBoys, List<Card> communityCards)
    {
        if (winnerHand != WinnerHand.None)
            return;
        winners = new List<Winner>();
        winners.Clear();
        winnerIndex = 0;

        for (int i = 0; i < playerBoys.Count; i++)
        {
            winnerHand = WinnerHand.High_Card;
            Winner win = new Winner();
            win.playerIndex = i;
            win.cardSet = playerBoys[i].playerCards.OrderBy(d => d.rank).ToList(); ;
            winners.Add(win);

        }
        int highest = 0;
        for (int i = 0; i < winners.Count; i++)
        {
            if (winners[i] != null)
            {
                if ((int)winners[i].cardSet[0].rank > highest)
                {
                    highest = (int)winners[i].cardSet[0].rank;
                    winnerIndex = winners[i].playerIndex;
                }
            }
        }
    }
    #endregion


    [SerializeField]  List<string> combinationList;
     void Combinations(int data)
     {
        //var sequence = new[] { 0, 1, 2, 3, 4 };
        List<int> sequenceList = new List<int>();
        for (int i = 0; i < data; i++)
        {
            sequenceList.Add(i);
        }
        var sequence = sequenceList.ToArray();
        sequence.Combinations(3);
        foreach (var combination in sequence.Combinations(3))
        {
            //Debug.Log(string.Join(",", combination));
            combinationList.Add(string.Join(",", combination));
        }
     }

    #region TESTING_FUNCTIONS
    void TestScenario()
    {
        //Combinations(5);
        GetWinner(playersList, communityCardsList);
    }
    void TestDebugs()
    {
        Debug.Log(winnerHand);
        switch (winnerHand)
        {
            case WinnerHand.Royal_Flush:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Royal Flush");
                break;
            case WinnerHand.Straight_Flush:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with straight Flush");
                break;
            case WinnerHand.Four_Of_A_Kind:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Four Of A Kind");
                break;
            case WinnerHand.Full_House:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Full House");
                break;
            case WinnerHand.Flush:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Flush");
                break;
            case WinnerHand.Straight:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Straight");
                break;
            case WinnerHand.Three_Of_A_Kind:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Three of a kind");
                break;
            case WinnerHand.Two_Pair:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Two Pair");
                break;
            case WinnerHand.Pair:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with Pair");
                break;
            case WinnerHand.High_Card:
                Debug.Log("PLayer " + winnerIndex + " is the Winner with High Card");
                break;
            case WinnerHand.None:
                break;
            default:
                break;
        }
    }
    #endregion
}

[Serializable]
public class Winner
{
    public int playerIndex;
    public List<Card> cardSet = new List<Card>();
}