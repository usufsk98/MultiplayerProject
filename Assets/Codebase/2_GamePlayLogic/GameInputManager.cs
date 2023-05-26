using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager instance;

    public static Action ShowUI;
    public static Action HideUI;

    public Button fold;
    public Button check;
    public Button call;
    public Button raise;

    public GameObject radialSlider;
    public GameObject raisePanel;
    public Button done;
    public Slider slider;

    public GameObject UIPanel;

    public TextMeshProUGUI betValueText;
    public int localbetValue;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        fold.onClick.AddListener(() => Fold());
        //check.onClick.AddListener(() => Check());
        //call.onClick.AddListener(() => Call());
        //done.onClick.AddListener(() => DealDone());
    }

    public void Show()
    {
        Invoke(nameof(DelayShoW), 0.3f);
    }

    public void DelayShoW() => UIPanel.SetActive(true);
    public void Hide() => UIPanel.SetActive(false);

    private void OnEnable()
    {
        ShowUI += Show;
        HideUI += Hide;
    }

    private void OnDisable()
    {
        ShowUI -= Show;
        HideUI -= Hide;
    }
    public void Fold()
    {
        Debug.Log("Player folds.");
        Dealer.instance.currentPlayerBoy.Fold();
    }


    public void Call()
    {
        Debug.Log("Player Calls.");
        DelayUI();
        Dealer.instance.currentPlayerBoy.Call(Dealer.instance.CurrentBet);
    }

    public void Check()
    {
        // player checks, no additional bet amount
        Debug.Log("Player checks.");
        DelayUI();
        Dealer.instance.currentPlayerBoy.Check();
    }

    public void DelayUI()
    {
        HideUI?.Invoke();
        Invoke(nameof(Turn), 2f);
    }

    public void Turn() => ShowUI?.Invoke();

    // ------------ Raise Panel for Now ----------
    public void DealDone()
    {
        if (Dealer.instance.currentPlayerBoy.PlayerChips >= localbetValue)
        {
            Dealer.instance.currentPlayerBoy.RaiseFactor(localbetValue);
            raisePanel.SetActive(false);

            DelayUI();
        }
        //else
        //{
        //    Debug.Log("Player doesn't have enough chips to raise.");
        //}
    }

    public void AddBet()
    {
        localbetValue += 50;
        betValueText.text = localbetValue.ToString();
    }

    public void SubtractBet()
    {
        if (localbetValue > 0 && localbetValue > localLockValue)
        {
            localbetValue -= 50;
            betValueText.text = localbetValue.ToString();
        }
    }

    int localLockValue;
    public void SetValue(int currentBet)
    {
        localbetValue = currentBet;
        localLockValue = currentBet;

        betValueText.text = localbetValue.ToString();
    }
}
