using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    //Keys
    public static string WinStatKey = "winStat";
    public static string  WinStatValue = "0";
    public static string CountryKey = "Country";
    public static int countryValue = 0;


    public static string UserCodeKey = "UserCodename";
    public static string UserCodeValue = "";

    public static string UserNameKey = "Username";
    public static string UserNameValue = "";

    static string IsLoggedInGuestKey = "IsLoggedInGuest";
    public static string IsloggedInGuestValue = "";
   
    static string FacebookTokenKey = "FacebookToken";
    static string IsloggedinFacebookKey = "";
    static string GoogleTokenKey = "GoogleToken";
    static string EmailForGoogleKey = "GoogleEmail";
    static string IsLoggedInGoogleKey = "IsLoggedIn";

    public static string EmailForGoogleValue = "";
    public static string FacebookTokenValue = "";
    public static string IsloggedinFacebookValue = "";
    static string GoogleTokenValue = "";
    public static string IsloggedInGoogleValue = "";


    public static string IsloginWithAppleKey = "AppleKey";
    public static string IsLoggedInAppleValue = "";
    
    public static string RewardTimeKey = "RewardTime";
    public static string RewardDayKey = "RewardDay";

    
    public static string RewardTimeValue = "";
    public static string RewardDayValue = "1";


    //Settings
    public static string SoundKey = "SoundKey";
    public static int Soundvalue = 1;
    public static string VibrationKey = "VibrationKey";
    public static int VibrationValue = 1;
    public static string NotificationKey = "NotificationKey";
    public static int NotificationValue = 1;
    public static string FourColorDeckKey = "FourColorDeckKey";
    public static int FourColorDeckValue = 1;
    public static string FreeChipsKey = "FreeChipsKey";
    public static int FreeChipsValue = 1;
    public static string PublicChallengeKey = "PublicChallengeKey";
    public static int PublicChallengeValue = 1;

    public static string ChipsKey = "ChipsKey";
    public static int ChipsValue = 0;

    private void Awake()
    {
        //ChipsValue = 10000;
        //SaveEconomy();
        Load();
        Save();
        DontDestroyOnLoad(gameObject);
    }
    public static void Save()
    {
        PlayerPrefs.SetString(UserCodeKey,UserCodeValue);
        PlayerPrefs.SetString(UserNameKey,UserNameValue);

        PlayerPrefs.SetString(IsLoggedInGuestKey, IsloggedInGuestValue);
        PlayerPrefs.SetString(IsLoggedInGoogleKey, IsloggedInGoogleValue);
        PlayerPrefs.SetString(IsloginWithAppleKey, IsLoggedInAppleValue);
        PlayerPrefs.SetString(FacebookTokenKey, FacebookTokenValue);
        PlayerPrefs.SetString(IsloggedinFacebookKey, IsloggedinFacebookValue);
        PlayerPrefs.SetString(EmailForGoogleKey, EmailForGoogleValue);
        PlayerPrefs.SetString(GoogleTokenKey, GoogleTokenValue);

        PlayerPrefs.SetString(RewardDayKey, RewardDayValue);
        PlayerPrefs.SetString(RewardTimeKey, RewardTimeValue);

        //Settings
        PlayerPrefs.SetInt(SoundKey, Soundvalue);
        PlayerPrefs.SetInt(VibrationKey, VibrationValue);
        PlayerPrefs.SetInt(NotificationKey, NotificationValue);
        PlayerPrefs.SetInt(FourColorDeckKey, FourColorDeckValue);
        PlayerPrefs.SetInt(FreeChipsKey, FreeChipsValue);
        PlayerPrefs.SetInt(PublicChallengeKey, PublicChallengeValue);
        PlayerPrefs.SetInt(ChipsKey, ChipsValue);
        PlayerPrefs.SetInt(CountryKey, countryValue);

        PlayerPrefs.Save();
    }

    public static void SaveEconomy()
    {
        PlayerPrefs.SetInt(ChipsKey, ChipsValue);
    }
    public static void Load()
    {
        UserCodeValue = PlayerPrefs.GetString(UserCodeKey);
        UserNameValue = PlayerPrefs.GetString(UserNameKey);
      
        IsloggedInGuestValue = PlayerPrefs.GetString(IsLoggedInGuestKey);
        IsLoggedInAppleValue = PlayerPrefs.GetString(IsloginWithAppleKey);
        
        IsloggedInGoogleValue = PlayerPrefs.GetString(IsLoggedInGoogleKey);
        FacebookTokenValue = PlayerPrefs.GetString(FacebookTokenKey);
        IsloggedinFacebookValue = PlayerPrefs.GetString(IsloggedinFacebookKey);
        GoogleTokenValue = PlayerPrefs.GetString(GoogleTokenKey);
        EmailForGoogleValue = PlayerPrefs.GetString(EmailForGoogleKey);
      
        RewardDayValue = PlayerPrefs.GetString(RewardDayKey);
        RewardTimeValue = PlayerPrefs.GetString(RewardTimeKey);

        //Settings
        Soundvalue = PlayerPrefs.GetInt(SoundKey,1);
        VibrationValue = PlayerPrefs.GetInt(VibrationKey,1);
        NotificationValue = PlayerPrefs.GetInt(NotificationKey,1);
        FourColorDeckValue = PlayerPrefs.GetInt(FourColorDeckKey,1);
        FreeChipsValue = PlayerPrefs.GetInt(FreeChipsKey,1);
        PublicChallengeValue = PlayerPrefs.GetInt(PublicChallengeKey,1);

        ChipsValue = PlayerPrefs.GetInt(ChipsKey);
        countryValue = PlayerPrefs.GetInt(CountryKey);

    }

    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
        Save();
    }

    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
        Save();
    }

    public static void ClearLoginCredentials()
    {
        UserCodeValue = "";
        IsloggedInGoogleValue = "";
        FacebookTokenValue = "";
        IsloggedinFacebookValue = "";
        GoogleTokenValue = "";
        IsloggedInGoogleValue = false.ToString();
        IsloggedinFacebookValue = false.ToString();

        PlayerPrefs.Save();
    }

}
