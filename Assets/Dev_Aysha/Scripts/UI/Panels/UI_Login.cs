using System;
using System.Diagnostics;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Login : UI_Panel
{
        public void LoginWithGoogle()
        {

        }
        public void LoginWithFacebook()
        {

        }
        public void ContinueWithGuest()
        {
            UI_Manager.instance.loader.StartGame();
            string username = "Guest" + UnityEngine.Random.Range(0, 10000);
            PlayerPrefsManager.UserCodeValue = username;
            PlayerPrefsManager.IsloggedInGuestValue = true.ToString();
            PlayerPrefsManager.Save();
            UI_Manager.instance.OpenPanel(typeof(UI_MainMenu),true);
        }
}


