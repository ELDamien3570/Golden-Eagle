using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string ObjectName;
    public Sprite ObjectIcon;
    public enum SlotType
    {
        Food,
        Weapon,
        Helmet,
        Shoulder,
        Chest,
        Legs,
        Feet
    }

    public SlotType itemType;
    public GameObject itemPrefab;
    public GameObject draggableIcon;
}
