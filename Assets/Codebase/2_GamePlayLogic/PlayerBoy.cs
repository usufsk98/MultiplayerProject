using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum PlayerAction
{
    None, // Default action when player has not acted yet
    Fold, // Player folds their hand and exits the current hand
    Check, // Player checks (i.e., bets zero) if no previous bets have been made
    Call, // Player matches the previous bet to stay in the hand
    Bet, // Player makes a new bet (higher than the previous bet) to start a new round of betting
    Raise, // Player raises the previous bet to a higher amount
    AllIn // Player bets all their remaining chips
}

public class PlayerBoy : MonoBehaviour
{
    public enum PlayerRank { Dealer, SmallBlind, BigBlind }

    public PlayerRank playerRankEnum;

    public List<Card> playerCards;
    public List<Transform> playerCardsPosition;

    public List<GeneratedCard> generatedCards;
    public GameObject cardPrefab;

    public TextMeshProUGUI playerChipsText;
    public TextMeshProUGUI betChipsText;
    public Image highlightedImage;

    public int playerChips = 8000;
    public int betChips = 0;

    public PlayerAction currentPlayerAction;
    public GameObject winnerPanel;
    public GameObject dealerTag;

    public bool dealerBool;
    public bool bigBlindBool;
    public bool smallBlindBool;

    public int lastBetLocalPlayer;

    public int betValueCurrent;

    public int playerCurrentTotalBet;

    public TextMeshProUGUI nameText;

    public bool myTurn;
    public PhotonView photonView;

    private void Start()
    {
        winnerPanel.SetActive(false);
        dealerTag.SetActive(false);
        betChips = 0;
        currentPlayerAction = PlayerAction.None;
        UpdateUITexts();
        PlayerChips = PlayerPrefs.GetInt(this.gameObject.name, 8000);
        Debug.Log(gameObject.name + PlayerChips);
        photonView = GetComponent<PhotonView>();

        
        //PlayerRankSetter();
    }

    public void PlayerRankSetter()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            playerRankEnum = PlayerRank.Dealer;
            Debug.Log("Player Rank is Dealer Local");
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            playerRankEnum = PlayerRank.BigBlind;
            Debug.Log("Player Rank is Big Blind Local");
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            playerRankEnum = PlayerRank.SmallBlind;
            Debug.Log("Player Rank is Small Blind Local");
        }
    }

    [PunRPC]
    public void PlayerRankSetterRPC()
    {
        if (photonView != null)
        {
            if (!photonView.IsMine)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient && !photonView.IsMine)
                {
                    playerRankEnum = PlayerRank.Dealer;
                    Debug.Log("Player Rank is Dealer RPC");
                }
                else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && !photonView.IsMine)
                {
                    playerRankEnum = PlayerRank.BigBlind;
                    Debug.Log("Player Rank is Big Blind RPC");
                }
                else if (PhotonNetwork.LocalPlayer.ActorNumber == 3 && !photonView.IsMine)
                {
                    playerRankEnum = PlayerRank.SmallBlind;
                    Debug.Log("Player Rank is Small Blind RPC");
                }
            }
        }   
    }


    public void SetScore()
    {
        PlayerPrefs.SetInt(this.gameObject.name, PlayerChips);
        Debug.Log(gameObject.name + PlayerChips);
    }

    public void Winner()
    {
        //Commented by Dev
        //winnerPanel.SetActive(true);
    }

    public int PlayerChips
    {
        get { return playerChips; }
        set
        {
            //playerChips = value;
            //UpdateUITexts();
        }
    }
    public int BetChips
    {
        get {
            return betChips; 
        }
        set
        {
            //betChips = value;
            //UpdateUITexts();
        }
    }

    public void UpdateUITexts()
    {
        playerChipsText.text = "" + playerChips.ToString();
        betChipsText.text = "" + betChips.ToString();
        //photonView.RPC("UpdateUITextsRPC", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateUITextsRPC(string chipsBet)
    {
        if(photonView != null)
        {
            if (!photonView.IsMine)
            {
                Debug.LogError(chipsBet);
                //playerChipsText.text = "" + playerChips.ToString();
                betChipsText.text = "" + chipsBet;
            }
        }
        
    }
    public void AddCard(Card card/*, Transform dealerPosition*/)
    {
        GeneratedCard generatedCard = cardPrefab.GetComponent<GeneratedCard>();
        generatedCard.SetValue(card);
        generatedCard.RavealCard(false);

        playerCards.Add(card);
        GameObject cardObj = Instantiate(cardPrefab, this.transform);
        cardObj.transform.DOMove(playerCardsPosition[playerCards.Count - 1].position, 1f);
        cardObj.transform.DORotateQuaternion(playerCardsPosition[playerCards.Count - 1].rotation, 1f);

        generatedCards.Add(cardObj.GetComponent<GeneratedCard>());
    }

    [PunRPC]
    public void InitialTurnRPC(int value)
    {
        if (photonView != null)
        {
            if (!photonView.IsMine)
            {
                int animationTimes = 1;
                AnimationManager.instance.PlayAnimation(playerChipsText.transform, betChipsText.transform, animationTimes);
                StartCoroutine(ChangeValue(animationTimes, value / 1));
            }
        }
    }


    public void InitialTurn(int value, int animationTimes = 1)
    {
        AnimationManager.instance.PlayAnimation(playerChipsText.transform, betChipsText.transform, animationTimes);
        StartCoroutine(ChangeValue(animationTimes, value / animationTimes));
    }
    IEnumerator ChangeValue(int animationTimes, int value)
    {
        int delayFactor = animationTimes;
        while (animationTimes > 0)
        {
            yield return new WaitForSeconds(1.2f);
            playerChips -= value;
            betChips += value;
            animationTimes--;
        }
        yield return new WaitForSeconds(delayFactor * 1f);
        Dealer.instance.currentBet = 0;
        //Dealer.instance.NextPlayerTurn();
    }

    public void Decide()
    {
        if (currentPlayerAction == PlayerAction.Fold) return;
        // Decide Fold , Check or Raise
    }

    public void Fold()
    {
        currentPlayerAction = PlayerAction.Fold;
        this.GetComponent<Image>().DOFade(0.3f, 0.3f);
        foreach (GeneratedCard g in generatedCards)
            g.gameObject.SetActive(false);
        playerChipsText.gameObject.SetActive(false);
        betChipsText.gameObject.SetActive(false);
        //Dealer.instance.NextPlayerTurn();

        if (Dealer.instance.CheckAllFold() == 1)
            Dealer.instance.GameCompleted();
    }

    // Method to handle a check action
    public void Check()
    {
        currentPlayerAction = PlayerAction.Check;
        InitialTurn(Dealer.instance.currentBet, 1);

        Debug.Log(Dealer.instance.currentBet);
        Debug.Log("Check Function");
    }

    // Method to handle a raise action
    public void RaiseFactor(int raisedBetFactor)
    {
        currentPlayerAction = PlayerAction.Raise;
        //Commented By Dev
        //Dealer.instance.currentBet = raisedBetFactor + Dealer.instance.currentPlayerBoy.betChips;
        Dealer.instance.currentBet = raisedBetFactor;
        InitialTurn(raisedBetFactor);

        Debug.Log("Check Function");
    }

    // Method to handle a call action

    public void Call(int currentBet)
    {
        int callAmount = currentBet;
        //if (callAmount > playerChips)
        //{
        //    // Player cannot call because they do not have enough chips
        //    // Prompt player to fold or go all-in
        //}
        //else
        {
            currentPlayerAction = PlayerAction.Call;
            // Subtract the call amount from the player's chips
            playerChips -= callAmount;
            // Add the call amount to the current bet
            currentBet += callAmount;
            // Update the UI to reflect the new pot size and player chip counts
            // Prompt the next player to act
            
        }

        Debug.Log("Call Function");
    }

    public void Showdown()
    {
        foreach (GeneratedCard card in generatedCards)
            card.RavealCard(true);
    }

    public void ResetAction()
    {
        betChips = 0;
        if (currentPlayerAction == PlayerAction.Fold) return;
        currentPlayerAction = PlayerAction.None;
    }


    public void PlayerTurn()
    {
        GameInputManager.instance.DealDone();
        Debug.Log(Dealer.instance.currentBet);
        Dealer.instance.currentPlayerBoy.playerChips -= GameInputManager.instance.localbetValue;
        Dealer.instance.currentPlayerBoy.betChips += GameInputManager.instance.localbetValue;
        UpdateUITexts();
        //if (bigBlindBool && Dealer.instance.currentBet >= 200)
        //{
        //    Dealer.instance.currentPlayerBoy.playerChips -= GameInputManager.instance.localbetValue;
        //    Dealer.instance.currentPlayerBoy.betChips += GameInputManager.instance.localbetValue;
        //    UpdateUITexts();
        //}
        //else if (smallBlindBool && Dealer.instance.currentBet >= 400)
        //{
        //    Dealer.instance.currentPlayerBoy.playerChips -= GameInputManager.instance.localbetValue;
        //    Dealer.instance.currentPlayerBoy.betChips += GameInputManager.instance.localbetValue;
        //    UpdateUITexts();
        //}
        //else
        //{
        //    Dealer.instance.currentPlayerBoy.playerChips -= GameInputManager.instance.localbetValue;
        //    Dealer.instance.currentPlayerBoy.betChips += GameInputManager.instance.localbetValue;
        //    UpdateUITexts();
        //}
    }

}


public static class ColorExtensions
{
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}