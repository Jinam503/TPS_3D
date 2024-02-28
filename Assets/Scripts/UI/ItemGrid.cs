using JetBrains.Annotations;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float TileSizeWidth = 106;
    public const float TileSizeHeight = 106;

    private InventoryItem[,] inventoryItemSlot;

    private RectTransform rectTransform;
    
    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
        rectTransform.sizeDelta = size;
    }
    
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        Vector2 positionOnGrid;
        Vector2Int tileGridPosition = default;
        
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnGrid.x / TileSizeWidth);
        tileGridPosition.y = (int)(positionOnGrid.y / TileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height))
            return false;

        if (!OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem)
        {
            CleanGridReference(overlapItem);
        }
        
        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform inventoryItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        inventoryItemRectTransform.SetParent(rectTransform);

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        inventoryItemRectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * TileSizeWidth + TileSizeWidth * inventoryItem.Width / 2;
        position.y = -(posY * TileSizeHeight + TileSizeHeight * inventoryItem.Height / 2);
        return position;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (!toReturn) return null;

        CleanGridReference(toReturn);
        
        return toReturn;
    }

    private void CleanGridReference(InventoryItem inventoryItem)
    {
        for (int ix = 0; ix < inventoryItem.Width; ix++)
        {
            for (int iy = 0; iy < inventoryItem.Height; iy++)
            {
                inventoryItemSlot[inventoryItem.onGridPositionX + ix, inventoryItem.onGridPositionY + iy] = null;
            }
        }
    }

    private bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0 || posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (!PositionCheck(posX, posY)) return false;

        posX += width -1;
        posY += height -1;

        if (!PositionCheck(posX, posY)) return false;
        
        return true;
    }

    public bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y])
                {
                    if (!overlapItem)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemSlot[posX + x, posY + y])
                            return false;
                    }
                }
            }
        }

        return true;
    }
    
    public bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot?[posX + x, posY + y])
                {
                    return false;
                }
            }
        }

        return true;
    }

    [CanBeNull]
    public InventoryItem GetItem(int x, int y)
    {
        if (PositionCheck(x,y))
        {
            return inventoryItemSlot[x, y];
        }
        else
        {
            return null;
        }
    }


    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.Height + 1;
        int width = gridSizeWidth - itemToInsert.Width + 1;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.Width, itemToInsert.Height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}
