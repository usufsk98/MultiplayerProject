using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


// Number of scenes should match in spellings with this enum
public enum SceneType { Loader, MainScene, GamePlay }; 

public class Loader : Singleton_IndependentObject<Loader>
{
    [HideInInspector] public bool managersLock;

    //[SerializeField] private GameObject dataManager;
    //[SerializeField] private GameObject iapManager;

    //[SerializeField] private GameObject gameManager;
    //[SerializeField] private GameObject audioManager;

    [SerializeField] private GameObject loadingOverlay;

    public void StartGame() => StartCoroutine(LoadManagers());

    private IEnumerator LoadManagers()
    {
        loadingOverlay.SetActive(true);
        //yield return LoadManager(dataManager);
        //yield return LoadManager(iapManager);
        //yield return LoadManager(gameManager);
        //yield return LoadManager(audioManager);

        // yield return LoadManager(menusManager);

        // --------Tesiing -----------------------
        yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene(SceneType.GamePlay.ToString());
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(1f);
        loadingOverlay.SetActive(false);
        // --------Tesiing -----------------------
    }

    //private IEnumerator LoadManager(IParent manager, GameObject gameObject)
    //{
    //    Assert.IsNotNull(gameObject);
    //    Assert.IsNull(manager);

    //    Debug.Log("Loading " + gameObject.name);
    //    Instantiate(gameObject);
    //}
}