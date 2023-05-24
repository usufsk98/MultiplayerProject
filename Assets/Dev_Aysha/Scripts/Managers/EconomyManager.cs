using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    public int chips;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        PlayerPrefsManager.Load();
        //Debug.Log("CHIPS" + PlayerPrefsManager.ChipsValue);
        chips = PlayerPrefsManager.ChipsValue;
    }
    public void SetEconomy(int _value)
    {
        chips = chips+ _value;
        //Debug.Log(chips + ": " + _value);
        PlayerPrefsManager.ChipsValue = chips;
        //Debug.LogError(PlayerPrefsManager.ChipsValue);
        PlayerPrefsManager.Save();
    }
    public int GetEconomy()
    {
        //PlayerPrefsManager.Load();
        //chips = PlayerPrefsManager.ChipsValue;
        //Debug.LogError("Chips: "+chips);
        return chips;
    }
}
