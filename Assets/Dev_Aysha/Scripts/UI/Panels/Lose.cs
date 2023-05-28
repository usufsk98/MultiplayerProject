using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public void Ok()
    {
        PhotonManager.instance.LeaveGameRoom();
        GameManager.instance.GameEnd();
    }
}
