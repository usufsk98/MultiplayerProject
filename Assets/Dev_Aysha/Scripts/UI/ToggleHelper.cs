using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToggleHelper : MonoBehaviour
{
    [SerializeField] SettingsToggleType SettingsToggle;
    [SerializeField] GameObject on;
    [SerializeField] GameObject Off;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        PlayerPrefsManager.Load();
        switch (SettingsToggle)
        {
            case SettingsToggleType.None:
                break;
            case SettingsToggleType.Sounds:
                GetToggleObject(PlayerPrefsManager.Soundvalue);
                break;
            case SettingsToggleType.Vibration:
                GetToggleObject(PlayerPrefsManager.VibrationValue);
                break;
            case SettingsToggleType.Notifications:
                GetToggleObject(PlayerPrefsManager.NotificationValue);
                break;
            case SettingsToggleType.FreeChips:
                GetToggleObject(PlayerPrefsManager.FreeChipsValue);
                break;
            case SettingsToggleType.FourColorDeck:
                GetToggleObject(PlayerPrefsManager.FourColorDeckValue);
                break;
            case SettingsToggleType.PublicChallengeResult:
                GetToggleObject(PlayerPrefsManager.PublicChallengeValue);
                break;
            default:
                break;
        }
    }
    public void SetToggleObject(int val)
    {
        if (val==1)
        {
            on.SetActive(false);
            Off.SetActive(true);
            Save(0);
        }
        else
        {
            on.SetActive(true);
            Off.SetActive(false);
            Save(1);
        }
        
    }
    void Save(int val)
    {
        switch (SettingsToggle)
        {
            case SettingsToggleType.None:
                break;
            case SettingsToggleType.Sounds:
                PlayerPrefsManager.Soundvalue = val;
                break;
            case SettingsToggleType.Vibration:
                PlayerPrefsManager.VibrationValue = val;
                break;
            case SettingsToggleType.Notifications:
                PlayerPrefsManager.NotificationValue = val;
                break;
            case SettingsToggleType.FreeChips:
                PlayerPrefsManager.FreeChipsValue = val;
                break;
            case SettingsToggleType.FourColorDeck:
                PlayerPrefsManager.FourColorDeckValue = val;
                break;
            case SettingsToggleType.PublicChallengeResult:
                PlayerPrefsManager.PublicChallengeValue = val;
                break;
            default:
                break;
        }
        PlayerPrefsManager.Save();
    }
    void GetToggleObject(int val)
    {
        if (val==1)
        {
            on.SetActive(true);
            Off.SetActive(false);
        }
        else
        {
            on.SetActive(false);
            Off.SetActive(true);
        }
    }
}
