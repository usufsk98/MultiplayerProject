using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : UI_Panel
{
    //[SerializeField] Toggle Sound_Toggle;
    void Start()
    {
        //Sound_Toggle.onValueChanged.AddListener(delegate
        //{
        //    SoundToggleValueChanged(Sound_Toggle);
        //});
    }

    //void SoundToggleValueChanged(Toggle change)
    //{
    //    PlayerPrefsManager.Soundvalue = change.isOn.ToString();
    //    PlayerPrefsManager.Save();
    //}
    
    public void Back()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_MainMenu), true);
    }
    public void Signout()
    {
        PlayerPrefs.DeleteAll();
        UI_Manager.instance.CloseAllPanels();
        UI_Manager.instance.OpenPanel(typeof(UI_Login), true);
    }
    public void DeleteAccount()
    {
        PlayerPrefs.DeleteAll();
        UI_Manager.instance.CloseAllPanels();
        UI_Manager.instance.OpenPanel(typeof(UI_Login), true);
    }
}
