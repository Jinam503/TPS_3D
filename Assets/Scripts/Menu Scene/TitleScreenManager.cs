using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        Debug.Log("Start");
        StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
    }
}
