using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public Item item;
    
    public int quantity;
    
    public int gridSizeWidth;
    public int gridSizeHeight;

    private TextMeshProUGUI quantityText_R;
    private TextMeshProUGUI quantityText_Nr;

    public TextMeshProUGUI QuantityText => rotated ? quantityText_R : quantityText_Nr;

    public int Height
    {
        get
        {
            if (!rotated)
                return gridSizeHeight;
            return gridSizeWidth;
        }
    }
    public int Width
    {
        get
        {
            if (!rotated)
                return gridSizeWidth;
            return gridSizeHeight;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;
    public void Set(Item item, int quantity, int width, int height)
    {
        this.item = item;   
        transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

        this.quantity = quantity;
        gridSizeWidth = width;
        gridSizeHeight = height;

        Vector2 size = new Vector2();
        size.x = Width * ItemGrid.TileSizeWidth - 6;
        size.y = Height * ItemGrid.TileSizeHeight - 6;
        GetComponent<RectTransform>().sizeDelta = size;

        quantityText_Nr = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        quantityText_R = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated ? 90f : 0f);
        quantityText_R.gameObject.SetActive(rotated);
        quantityText_Nr.gameObject.SetActive(!rotated);
    }

    private void Update()
    {
        QuantityText.text = quantity.ToString();
    }
}
