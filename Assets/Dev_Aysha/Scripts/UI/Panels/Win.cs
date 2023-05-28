using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void OK()
    {
        PhotonManager.instance.LeaveGameRoom();
        GameManager.instance.GameEnd();
    }
}
