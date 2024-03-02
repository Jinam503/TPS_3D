using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    
    [Header("MENUS")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("BUTTONS")]
    [SerializeField] private Button mainMenuNewGameButton;
    [SerializeField] private Button loadMenuReturnButton;
    [SerializeField] private Button mainMenuLoadGameButton;
    [SerializeField] private Button deleteCharacterPopUpConfirmButton;

    [Header("POP UPS")] 
    [SerializeField] private GameObject noCharacterSlotsPopUp;
    [SerializeField] private Button noCharacterSlotsOkayButton;
    [SerializeField] private GameObject deleteCharacterSlotPopUp;

    [Header("CHARACTER SLOTS")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.No_Slot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }
    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
    }
    public void CloseLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(true);
        titleScreenLoadMenu.SetActive(false);
    }
    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
    }
    public void CloseNoFreeChatacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
    }
    
    //  CHARACTER SLOTS
    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }
    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.No_Slot;
    }
    public void AtteptToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.No_Slot)
        {
            deleteCharacterSlotPopUp.SetActive(true);
        }
    }
    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
        
        //  TO REFRESH THE SLOTS
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
    }
    public void CloseDeleteCharacterPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
    }
}
