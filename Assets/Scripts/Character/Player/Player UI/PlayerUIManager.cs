using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    public PlayerManager player;
    
    [Header("Health")]
    public UI_HealthBar healthBar;

    [Header("GameMenu")] 
    public GameMenu gameMenu;
    
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
        currentAmmoCountText.text = player.playerEquipment.CurrentWeapon.remainingAmmo.ToString();
    }
}
