using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        DebugLog.Log("Start Game");
        
        UI_Manager.instance.CloseAllPanels();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        //UI_Manager.instance.OpenPanel(typeof(UI_Gameplay), true);
    }
}
