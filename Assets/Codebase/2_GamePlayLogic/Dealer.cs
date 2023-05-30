using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class Dealer : Singleton_IndependentObject<Dealer>
{
    private const int cardsToEachPlayer = 2;
    private const int cardsToCommunity = 5;

    public List<int> generatedCardsNumbers;

    public List<int> communityCardsIntegers;
    // -------- Deck ---------------
    public Deck deck;

    // -------- Players ---------------
    public List<PlayerBoy> players;
    public List<Card> comunityCards;
    public List<GeneratedCard> generatedCards;

    // -------- communityCard ---------------
    public List<Transform> communityCardsTransform;
    public GameObject communityCardPrefab;

    // -------- DealerInfo ---------------
    public int dealerChips = 0;
    public TextMeshProUGUI dealerText;
    public GameObject gameCompleted;


    int currentDealer = 0;

    //[HideInInspector]
    public PlayerBoy currentPlayerBoy;
    public PokerRoundManager pokerRoundManager;

    private int currentPlayer = 0;
    private Color previousColor;
    [SerializeField] PhotonView PhotonView;

    public void GameCompleted()
    {
        //GameInputManager.HideUI?.Invoke();
        //foreach (PlayerBoy playerBoy in players)
        //    playerBoy.Showdown();

        //List<PlayerBoy> playerBoys = new List<PlayerBoy>();
        //foreach (PlayerBoy boy in players)
        //{
        //    if (boy.currentPlayerAction != PlayerAction.Fold)
        //        playerBoys.Add(boy);
        //}

        //WinningScenerio wS = FindObjectOfType<WinningScenerio>();
        //PlayerBoy winnerPlayer = wS.GetWinner(playerBoys, comunityCards);
        //AnimationManager.instance.PlayAnimation
        //    (dealerText.transform, winnerPlayer.betChipsText.transform, 1);
        //StartCoroutine(CouroutineGameCompleted(winnerPlayer));
    }
    IEnumerator CouroutineGameCompleted(PlayerBoy winnerPlayer)
    {
        yield return new WaitForSeconds(0.5f);
        winnerPlayer.PlayerChips += dealerChips;
        winnerPlayer.Winner();
        Debug.Log(winnerPlayer.name + " Won!");
        foreach (PlayerBoy playerBoy in players)
            playerBoy.SetScore();

        PlayerPrefs.SetInt("currentDealer", Dealer.instance.currentDealer++);
        yield return new WaitForSeconds(5f);
        ReloadCurrentScene();
    }
    public int DealerChips
    {
        get { return dealerChips; }
        set
        {
            //dealerChips = value;
            //dealerText.text = "Pot: " + dealerChips;
        }
    }


    public int currentBet;
    public int CurrentBet
    {
        get { return currentBet; }
        set
        {
            //currentBet = value;
            //Debug.Log("--- Current " + currentBet);
        }
    }


    void Start()
    {
        OnlineMultiplayerManager.instance.PopulatePlayers();
        FillIntegerList();
        GettingFiveNumbers();
        //currentDealer = PlayerPrefs.GetInt("currentDealer", 0);
        //players = GameInfo.RotateLeft(players, currentDealer);
        //gameCompleted.SetActive(false);
        //currentDealer++;
        pokerRoundManager.SetCurrentRound(RoundType.PreFlop);
        CurrentBet = 0;
        deck = new Deck();
        GeneratePlayersCard();

        //// ------ Players Should be declared as SB, BB
        //int currentPlayer = GameInfo.dealerindex;
        // deal three cards to each player


    }
    List<int> playerCardsIntegers = new List<int>();
    public void GeneratePlayersCard()
    {
        int numToRemove;
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            playerCardsIntegers.Clear();
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerCardsIntegers.Add(i);
                for (int j = 0; j < 5; j++)
                {
                   numToRemove = (UnityEngine.Random.Range(0, generatedCardsNumbers.Count));
                   playerCardsIntegers.Add(numToRemove);
                   //deck.cards.RemoveAt(numToRemove);

                }
                GetComponent<PhotonView>().RPC("GeneratePlayerCards", RpcTarget.All, playerCardsIntegers.ToArray());
            }
           
        }
    }
    [PunRPC]
    public void GeneratePlayerCards(int[] numbers)
    {
        Debug.LogError("GeneratePlayer CARDS." +numbers[0]);
        Debug.LogError("GeneratePlayer CARDS." +numbers[1]);
        Debug.LogError("GeneratePlayer CARDS." +numbers[2]);
        //numbers[0] is player number in photon list
        if (PhotonNetwork.PlayerList[numbers[0]].UserId.Equals(PhotonNetwork.LocalPlayer.UserId))
        {
            for (int i = 1; i < numbers.Length; i++)
            {
                Debug.LogError("Dech.cards: " + deck.cards);
                Debug.LogError("Dech.cards num: " + numbers[i]);
                currentPlayerBoy.AddCard(deck.cards[numbers[i]]);
                deck.cards.RemoveAt(numbers[i]);
            }
            
        }
       
    }
    [PunRPC]
    public void PlayGameStarter()
    {
        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {
        Debug.Log("TestingPlayGameCoroutine");
        GameInputManager.HideUI?.Invoke();
        //yield return new WaitForSeconds(1f);
        //players[currentPlayer].dealerTag.SetActive(true);
        //StartPlayerOne();
        //yield return new WaitForSeconds(1f);
        //NextPlayerTurn();
        //yield return new WaitForSeconds(0.5f);
        //players[currentPlayer].InitialTurn(200, 1);
        //yield return new WaitForSeconds(2.5f);
        //players[currentPlayer].InitialTurn(400, 2);
        yield return new WaitForSeconds(2f);
        StartCoroutine(GiveCardsInAManner());

        //CurrentBet = 400;
        //GameInputManager.instance.SetValue(currentBet);
    }

    public void FillIntegerList()
    {
        for (int i = 0; i < 52; i++)
        {
            generatedCardsNumbers.Add(i);
        }
    }

    public void GettingFiveNumbers()
    {
        int numToRemove = 0;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < 5; i++)
            {
                numToRemove = (UnityEngine.Random.Range(0, generatedCardsNumbers.Count));
                communityCardsIntegers.Add(numToRemove);
                generatedCardsNumbers.RemoveAt(numToRemove);
            }
            GetComponent<PhotonView>().RPC("GettingFiveNumbersRPC", RpcTarget.Others, communityCardsIntegers.ToArray());
        }
    }

    [PunRPC]
    public void GettingFiveNumbersRPC(int[] numbers)
    {
        communityCardsIntegers.AddRange(numbers);
    }

    public void DealingCommunityCards()
    {
        for (int i = 0; i < communityCardsIntegers.Count; i++)
        {
            Debug.Log(communityCardsIntegers[i]);
            AddCommunityCard(deck.cards[communityCardsIntegers[i]]);
            deck.cards.RemoveAt(communityCardsIntegers[i]);
        }
    }

    IEnumerator GiveCardsInAManner()
    {
        currentPlayerBoy = OnlineMultiplayerManager.instance._localPlayerDataManager.GetComponent<PlayerBoy>();
        int cardsDealt = 0;

        //Commented For Testing
        //while (cardsDealt < cardsToEachPlayer)
        //{
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        PlayerBoy currentPlayer = players[i];
        //        currentPlayer.AddCard(deck.DrawCard(), this.transform);
        //        yield return new WaitForSeconds(0.4f);
        //    }
        //    cardsDealt++;
        //}

        //Deal community cards
        //List<Card> communityCards = new List<Card>();
        //for (int i = 0; i < cardsToCommunity; i++)
        //{
        //    yield return new WaitForSeconds(0.3f);
        //    Card card = deck.DrawCard();
        //    AddCommunityCard(card);
        //    communityCards.Add(card);
        //}

        //Dealing Community Cards Custom
        DealingCommunityCards();

        yield return new WaitForSeconds(0.1f);
        GameInputManager.ShowUI?.Invoke();
    }

    public void AddCommunityCard(Card card)
    {
        GeneratedCard generatedCard = communityCardPrefab.GetComponent<GeneratedCard>();
        generatedCard.SetValue(card);
        generatedCard.RavealCard(false);

        comunityCards.Add(card);
        GameObject cardObj = Instantiate(communityCardPrefab, this.transform);
        cardObj.transform.DOMove(communityCardsTransform[comunityCards.Count - 1].position, 1f);
        cardObj.transform.DORotateQuaternion(communityCardsTransform[comunityCards.Count - 1].rotation, 1f);
        generatedCards.Add(cardObj.GetComponent<GeneratedCard>());
    }

    public void PreFlop()
    {
        generatedCards[0].RavealCard(true);
        generatedCards[1].RavealCard(true);
        generatedCards[2].RavealCard(true);
        // currentBet = 0;
    }

    public void Flop()
    {
        generatedCards[3].RavealCard(true);
        //currentBet = 0;
    }

    public void Turn()
    {
        generatedCards[4].RavealCard(true);
    }

    public void SaveColor(int value)
    {
        previousColor = players[value].highlightedImage.color;
    }

    public void ChangeColor(int value)
    {
        players[value].highlightedImage.enabled = false;
    }
    public void RevertColor(int value)
    {
        players[value].highlightedImage.enabled = true;
    }

    public void StartPlayerOne()
    {
        SaveColor(currentPlayer);
        ChangeColor(currentPlayer);
    }

    public int CheckAllFold()
    {
        int count = 0;
        foreach (PlayerBoy playerBoy in players)
        {
            if (playerBoy.currentPlayerAction != PlayerAction.Fold)
                count++;
        }
        return count;
    }

    [PunRPC]
    public void NextPlayerTurnRPC()
    {
        //Commented by Dev
        bool value = IsBettingRoundComplete();
        if (value)
        {
            Debug.Log("-----Entered");
            AddCoinsToPot();
            // Do something for the completion
            switch (pokerRoundManager.currentRound)
            {
                case RoundType.PreFlop:
                    PreFlop();
                    break;
                case RoundType.Flop:
                    Flop();
                    break;
                case RoundType.Turn:
                    Turn();
                    break;
                case RoundType.River:
                    GameCompleted();
                    return;
            }
            pokerRoundManager.SetCurrentRound((RoundType)((int)pokerRoundManager.currentRound + 1));
            foreach (PlayerBoy player in players)
                player.ResetAction();
            Dealer.instance.CurrentBet = 0;
        }

        //currentPlayer++;
        //currentPlayer = (currentPlayer % players.Count);
        //if (currentPlayer == 0) RevertColor(players.Count - 1);
        //else RevertColor(currentPlayer - 1);

        // Increment the current player's turn and wrap around to 1 if we reach the total number of players     
        //SaveColor(currentPlayer);
        //ChangeColor(currentPlayer);

        //currentPlayerBoy = players[currentPlayer];
        //Dev Comment
        //currentPlayerBoy = OnlineMultiplayerManager.instance._localPlayerDataManager.GetComponent<PlayerBoy>();
        if (currentPlayerBoy.currentPlayerAction == PlayerAction.Fold)
            NextPlayerTurn();
        else Debug.Log("Player Name" + currentPlayerBoy.name);
        GameInputManager.instance.SetValue(currentBet - currentPlayerBoy.BetChips);
    }


    public void NextPlayerTurn()
    {
        //Commented by Dev
        bool value = IsBettingRoundComplete();
        if (value)
        {
            Debug.Log("-----Entered");
            AddCoinsToPot();
            // Do something for the completion
            switch (pokerRoundManager.currentRound)
            {
                case RoundType.PreFlop:
                    PreFlop();
                    break;
                case RoundType.Flop:
                    Flop();
                    break;
                case RoundType.Turn:
                    Turn();
                    break;
                case RoundType.River:
                    GameCompleted();
                    return;
            }
            pokerRoundManager.SetCurrentRound((RoundType)((int)pokerRoundManager.currentRound + 1));
            foreach (PlayerBoy player in players)
                player.ResetAction();
            Dealer.instance.CurrentBet = 0;
        }

        //currentPlayer++;
        //currentPlayer = (currentPlayer % players.Count);
        //if (currentPlayer == 0) RevertColor(players.Count - 1);
        //else RevertColor(currentPlayer - 1);

        // Increment the current player's turn and wrap around to 1 if we reach the total number of players     
        //SaveColor(currentPlayer);
        //ChangeColor(currentPlayer);

        //currentPlayerBoy = players[currentPlayer];
        //Dev Comment
        currentPlayerBoy = OnlineMultiplayerManager.instance._localPlayerDataManager.GetComponent<PlayerBoy>();
        if (currentPlayerBoy.currentPlayerAction == PlayerAction.Fold)
            NextPlayerTurn();
        else Debug.Log("Player Name" + currentPlayerBoy.name);
        //GameInputManager.instance.SetValue(currentBet - currentPlayerBoy.BetChips);
    }

    public void AddCoinsToPot()
    {
        foreach (PlayerBoy player in players)
        {
            DealerChips += player.BetChips;
            player.BetChips = 0;
        }

        Debug.Log("Dealer add coins to pot function");
    }

    public bool IsBettingRoundComplete()
    {
        for (int a = 0; a < players.Count; a++)
        {
            if (players[a].currentPlayerAction == PlayerAction.Fold) continue;
            if (players[a].BetChips != CurrentBet || players[a].BetChips == 0)
                return false;
            Debug.Log(players[a].currentPlayerAction);
            //if (players[a].currentPlayerAction == PlayerAction.None /*|| player.BetChips != currentBet*/)
            //{
            //    return false;
            //}
        }
        return true;
    }
    public void ReloadCurrentScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }
}

public static class GameInfo
{
    public const int dealerindex = 0;
    public const int costForFirstBid = 100;
    public static int indexOfFirstBidder;

    static int GetNextPlayerIndex(int currentIndex)
    {
        int nextIndex = currentIndex + 1;
        if (nextIndex >= 5)
        {
            nextIndex = 0;
        }
        return nextIndex;
    }

    public static List<PlayerBoy> RotateLeft(List<PlayerBoy> list, int num)
    {
        int n = list.Count;
        num = num % n;
        for (int i = 0; i < num; i++)
        {
            PlayerBoy temp = list[0];
            for (int j = 0; j < n - 1; j++)
            {
                list[j] = list[j + 1];
            }
            list[n - 1] = temp;
        }
        return list;
    }
}

public enum RoundType
{
    PreFlop,
    Flop,
    Turn,
    River
}

[Serializable]
public class PokerRoundManager
{
    public RoundType currentRound;
    public TextMeshProUGUI roundText;

    public void SetCurrentRound(RoundType roundType)
    {
        currentRound = roundType;
        UpdateRoundText();
    }

    private void UpdateRoundText()
    {
        roundText.text = "" + currentRound.ToString();
    }
}