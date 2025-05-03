using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Sprite defaultSprite;
    public DraggableIconSlot draggableIconSlot;
    public InventoryItem inventoryItem;
    public bool slotFilled = false;
    public enum SlotType
    {
        Any, 
        Weapon,
        Helmet,
        Shoulder,
        Chest, 
        Legs, 
        Feet
    }

    public SlotType slotType;

    public bool isItemEquipped = false;
     

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {

            GameObject dropped = eventData.pointerDrag;
            draggableIconSlot = dropped.GetComponent<DraggableIconSlot>();

            if (slotType == SlotType.Any)
            {
                draggableIconSlot.parentAfterDrag = transform;
            }
            else if (draggableIconSlot.slotItem != null)
            {
                if (draggableIconSlot.slotItem.itemType.ToString() == slotType.ToString())
                {
                    draggableIconSlot.parentAfterDrag = transform;
                }         
            }
            else return;         
        }
    }
    public void Update()
    {
        //if (inventoryItem != null && transform.childCount == 0 && !slotFilled)
        //{           
        //    slotFilled = true;
        //    Instantiate (inventoryItem.draggableIcon, this.transform);
        //    inventoryItem = null;
        //}
        UpdateInventoryItem();
    }

    public void UpdateInventoryItem()
    {
        if (transform.childCount > 0)
        {
            inventoryItem = GetComponentInChildren<DraggableIconSlot>().slotItem;
            draggableIconSlot = this.GetComponentInChildren<DraggableIconSlot>();
            slotFilled = true;
        }
        else if (transform.childCount == 0)
        {
            inventoryItem = null;
            draggableIconSlot=null;
            slotFilled = false;
        }
    }

    public void ReceiveInventoryItem(InventoryItem receivedItem)
    {
        if (transform.childCount == 0 && !slotFilled)
        {
            slotFilled = true;         
            Instantiate(receivedItem.draggableIcon, this.transform);
            draggableIconSlot = this.GetComponentInChildren<DraggableIconSlot>();
            inventoryItem = null;
        }
    }
}
