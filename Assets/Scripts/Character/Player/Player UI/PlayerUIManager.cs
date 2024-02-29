using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Ammo")]
    public TextMeshProUGUI currentAmmoCountText;
    public TextMeshProUGUI reservedAmmoCountText;

    private void Awake()
    {
        if(instance == null)
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

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //  WE MUST SHUT DOWN, BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
            NetworkManager.Singleton.Shutdown();
            //  WE THEN RESTART, AS A CLIENT
            NetworkManager.Singleton.StartClient();
        }
    }
}
