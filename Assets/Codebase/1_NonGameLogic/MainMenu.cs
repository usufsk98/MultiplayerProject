using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    Loader loader;
    private void Start()
    {
        loader = FindObjectOfType<Loader>();
    }

    public void RequestForSignIn(MenuBtnType signInType)
    {
        switch (signInType)
        {
            case MenuBtnType.Facebook:
                {
                    // Do Task
                    break;
                }
            case MenuBtnType.Gmail:
                {
                    // Do Task
                    break;
                }
            case MenuBtnType.Guest:
                {
                    // Do Task
                    break;
                }
        }
        loader.StartGame();
    }
}