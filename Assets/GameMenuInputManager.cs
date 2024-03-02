using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuInputManager : MonoBehaviour
{
    private GameMenu gameMenu;
    private PlayerControls playerControls;

    public PlayerManager player;

    [Header("Inputs")]
    public bool openMenuInput;
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.UI.OpenMenu.performed += i => openMenuInput = true;
        }
        playerControls.Enable();
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Awake()
    {
        gameMenu = GetComponent<GameMenu>();
    }
    private void Update()
    {
        HandleOpenMenuInput();
    }

    private void HandleOpenMenuInput()
    {
        if (openMenuInput)
        {
            openMenuInput = false;
            gameMenu.HandleMenu();
        }

        player.playerInventory.isInventoryOpened = gameMenu.IsMenuOpened;
    }
}
