using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    [Header("World Scene Index")]
    [SerializeField] private int worldSceneIndex = 1;

    [Header("Current Chracter Data")]
    public CharacterSaveData currentChracterData;

    [Header("Chracter Slots")]
    public CharacterSaveData characterSlot01;
    //public CharacterSaveData characterSlot02;
    //public CharacterSaveData characterSlot03;
    //public CharacterSaveData characterSlot04;
    //public CharacterSaveData characterSlot05;
    //public CharacterSaveData characterSlot06;
    //public CharacterSaveData characterSlot07;
    //public CharacterSaveData characterSlot08;
    //public CharacterSaveData characterSlot09;
    //public CharacterSaveData characterSlot10;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
