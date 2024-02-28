using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameMenu;
    
    public PlayerInventory inventory;

    public bool IsMenuOpened => gameMenu.activeSelf;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        gameMenu.SetActive(false);
    }

    public void HandleMenu()
    {
        if (IsMenuOpened)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameMenu.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            gameMenu.SetActive(true);
        }
        //  This is Temp. It will be changed after Story Comes;
        inventory.isInventoryOpened = IsMenuOpened;
    }
}
