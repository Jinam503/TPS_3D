using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerInventory : MonoBehaviour
{
    private PlayerManager playerManager;
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            SetParent();
        }
    }
    
    [SerializeField] private ItemGrid selectedItemGrid;
    private InventoryItem selectedItem;
    private RectTransform selectedItemRectTransform;
    
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvusTransform;
    [SerializeField] private RectTransform highlighter;

    private InventoryItem overlapItem;
    
    public bool isInventoryOpened;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (!isInventoryOpened) return;
        Debug.Log(GetTileGridPosition());
        ItemIconDrag();
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (!SelectedItemGrid)
        {
            ShowHighlighter(false);
            return;
        }
        
        HandleHighlight();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (!selectedItem)
            return;

        selectedItem.Rotate();
        SetHighLighterSize(selectedItem);
        SetHighlighterPosition(selectedItem);
    }
    
    public void AddItem(Item item, int quantity, int width, int height)
    {
        if (!SelectedItemGrid)
            return;
        
        if (FindItem(item))
        {
            InventoryItem inventoryItem = FindItem(item);
            int amountOfLeftItem = inventoryItem.item.maxquantity - inventoryItem.quantity;

            if (quantity <= amountOfLeftItem)
            {
                inventoryItem.quantity += quantity;
            }   
            else
            {
                inventoryItem.quantity = inventoryItem.item.maxquantity;
                
                InventoryItem newInventoryItem = Instantiate(itemPrefab, canvusTransform).GetComponent<InventoryItem>();
                newInventoryItem.Set(item,quantity - amountOfLeftItem, width, height);
                inventoryItems.Add(newInventoryItem);
                InsertItem(inventoryItem);
            }
        }
        else
        {
            InventoryItem inventoryItem = Instantiate(itemPrefab, canvusTransform).GetComponent<InventoryItem>();
            inventoryItem.Set(item,quantity,width, height);
            
            inventoryItems.Add(inventoryItem);
            
            InsertItem(inventoryItem);
        }
        
        if (playerManager.playerEquipment.CurrentWeapon.ammotype == AmmoType.rifle && item.itemID == "AmmoRifleItem")
        {
            PlayerUIManager.instance.reservedAmmoCountText.text = GetAmountOfAmmoByAmmoType(AmmoType.rifle).ToString();
        }
        else if(playerManager.playerEquipment.CurrentWeapon.ammotype == AmmoType.pistol &&item.itemID == "AmmoPistolItem")
        {
            PlayerUIManager.instance.reservedAmmoCountText.text = GetAmountOfAmmoByAmmoType(AmmoType.pistol).ToString();
        }
        else if(playerManager.playerEquipment.CurrentWeapon.ammotype == AmmoType.shotgun && item.itemID == "AmmoShotgunItem")
        {
            PlayerUIManager.instance.reservedAmmoCountText.text = GetAmountOfAmmoByAmmoType(AmmoType.shotgun).ToString();
        }
    }

    private InventoryItem FindItem(Item item)
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (inventoryItem.item == item && inventoryItem.quantity != inventoryItem.item.maxquantity)
            {
                return inventoryItem;
            }
        }

        return null;
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
            return;
        SelectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
    private Vector2Int oldPosition;
    private InventoryItem itemTohighlight;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;
        if (!selectedItem)
        {
            itemTohighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemTohighlight)
            {
                ShowHighlighter(true);
                SetHighLighterSize(itemTohighlight);
                SetHighlighterPosition(itemTohighlight);
            }
            else
            {
                ShowHighlighter(false);
            }
        }
        else
        {
            ShowHighlighter(SelectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.Width,
                selectedItem.Height));
            SetHighLighterSize(selectedItem);
            SetHighlighterPosition(selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        //  Specify Scope
        if (!selectedItem)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem)
        {
            position.x -= (selectedItem.Width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (selectedItem.Height - 1) * ItemGrid.TileSizeHeight / 2;
        }
        
        
        return SelectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        if (SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem))
        {
            selectedItem = null;
            if (overlapItem)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
                selectedItemRectTransform.SetAsLastSibling();
            }
        }
    }
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }
    private void ItemIconDrag()
    {
        if (selectedItem)
        {
            selectedItemRectTransform.position = Input.mousePosition;
            highlighter.SetSiblingIndex(1);
        }
        
    }

    public void SetHighLighterSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.Width * ItemGrid.TileSizeWidth;
        size.y = targetItem.Height * ItemGrid.TileSizeHeight;
        highlighter.sizeDelta = size;
    }


    public void SetParent()
    {
        if (!SelectedItemGrid)
            return;
        
        highlighter.SetParent(SelectedItemGrid.GetComponent<RectTransform>());
    }

    public void ShowHighlighter(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }
    public void SetHighlighterPosition(InventoryItem targetItem)
    {
        Vector2 pos =
            SelectedItemGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;
        if (!selectedItem)
        {
            highlighter.SetAsLastSibling();
        }
    }
    
    public void SetHighlighterPosition(InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos =
            SelectedItemGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
        if (!selectedItem)
        {
            highlighter.SetAsLastSibling();
        }
    }

    public int GetAmountOfAmmoByAmmoType(AmmoType weaponItemAmmotype)
    {
        int total = 0;
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (weaponItemAmmotype == AmmoType.rifle && inventoryItem.item.itemID == "AmmoRifleItem")
            {
                total += inventoryItem.quantity;
            }
            else if(weaponItemAmmotype == AmmoType.pistol && inventoryItem.item.itemID == "AmmoPistolItem")
            {
                total += inventoryItem.quantity;
            }
            else if(weaponItemAmmotype == AmmoType.shotgun && inventoryItem.item.itemID == "AmmoShotgunItem")
            {
                total += inventoryItem.quantity;
            }
        }

        return total;
    }

    public int UseAmmoByAmmoType(WeaponItem weaponItem, int amountOfAmmoToReload)
    {
        int toReturn = 0;
        List<InventoryItem> ammos = new List<InventoryItem>();
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (weaponItem.ammotype == AmmoType.rifle && inventoryItem.item.itemID == "AmmoRifleItem")
            {
                ammos.Add(inventoryItem);
            }
            else if(weaponItem.ammotype == AmmoType.pistol && inventoryItem.item.itemID == "AmmoPistolItem")
            {
                ammos.Add(inventoryItem);
            }
            else if(weaponItem.ammotype == AmmoType.shotgun && inventoryItem.item.itemID == "AmmoShotgunItem")
            {
                ammos.Add(inventoryItem);
            }
        }
        
        //  Sort by number of bullets in descending order.
        ammos.Sort((a,b) => a.quantity.CompareTo(b.quantity));

        if (ammos[0].quantity > amountOfAmmoToReload)
        {
            ammos[0].quantity -= amountOfAmmoToReload;
            toReturn = amountOfAmmoToReload;
            
        }
        else if (ammos[0].quantity == amountOfAmmoToReload)
        {
            inventoryItems.Remove(ammos[0]);
            Destroy(ammos[0].gameObject);
            toReturn = amountOfAmmoToReload;
        }
        else{
            if (ammos.Count == 1)
            {
                inventoryItems.Remove(ammos[0]);
                
                Destroy(ammos[0].gameObject);
                toReturn = ammos[0].quantity;
            }
            else
            {
                ammos[1].quantity -= amountOfAmmoToReload - ammos[0].quantity;
                inventoryItems.Remove(ammos[0]);
                
                Destroy(ammos[0].gameObject);
                toReturn = amountOfAmmoToReload;
            }
        }

        return toReturn;
    }
}
