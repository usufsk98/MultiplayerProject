using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuBtnType { Facebook, Gmail, Guest }
public class MainMenuBtn : MonoBehaviour
{
    public MenuBtnType menuBtnType;
    private void Start()
    {
        MainMenu mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu == null) throw new System.Exception("Main Menu is missing");
        this.GetComponent<Button>().onClick.AddListener(() =>
          {
              Debug.Log("Played");
              mainMenu.RequestForSignIn(this.menuBtnType);
          });
    }
}
