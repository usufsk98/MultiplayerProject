using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Profile : UI_Panel
{
    [SerializeField] Text userName;
    [SerializeField] Text userCode;
    [SerializeField] Image country;
    [SerializeField] GameObject editNamePanel;
    [SerializeField] GameObject countryPanel;
    [SerializeField] InputField NameField;
    [SerializeField] List<Image> Countries;
    void Start()
    {
        PlayerPrefsManager.Load();
        countryPanel.SetActive(false);
        editNamePanel.SetActive(false);
        SetProfile();
    }

    public void ChangeCountry()
    {
        countryPanel.SetActive(true);
    }
    public void SelectCoutnry(int countryNo)
    {
        country.sprite = Countries[countryNo].sprite;
        PlayerPrefsManager.countryValue = countryNo;
        PlayerPrefsManager.Save();
        Close(countryPanel);
    }
    public void EditNamePanel()
    {
        editNamePanel.SetActive(true);
    }
   
    void SetProfile()
    {
        if (string.IsNullOrEmpty(PlayerPrefsManager.UserNameValue))
            PlayerPrefsManager.UserNameValue = "Guest User";
        userName.text = PlayerPrefsManager.UserNameValue;
        userCode.text = PlayerPrefsManager.UserCodeValue;
        country.sprite = Countries[PlayerPrefsManager.countryValue].sprite;
    }
    public void SaveProfile()
    {
        if (NameField.text != "")
        {
            PlayerPrefsManager.UserNameValue = NameField.text;
            userName.text = PlayerPrefsManager.UserNameValue;
        }

        PlayerPrefsManager.Save();
        Close(editNamePanel);
    }

    public void Close(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
    }
    public void Back()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_MainMenu), true);
    }
}
