using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNetWorkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        Debug.Log("Start");
        StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
    }
}
