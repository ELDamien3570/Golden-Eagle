using System.Linq;
using UnityEngine;
using VHierarchy.Libs;

public class PlayerEquipmentManager : MonoBehaviour
{
    public PlayerInventoryManager inventory;
    public ItemSlot[] weaponSlots;
    public GameObject weaponEquipParent;   
    
    public Transform weaponEquipOffset;
    public int currentHeldWeapon = 0;
    private void Start()
    {
        inventory = GetComponent<PlayerInventoryManager>();
        
    }
    
    private void Update()
    {
       UpdateWeaponSlots();
    }

    private void UpdateWeaponSlots()
    {
        weaponSlots = inventory.characterManager.characterWeaponSlots.ToArray();
    }


    public void EquipWeapon(int incomWeapon)
    {
        int weaponSlotCorrected = incomWeapon - 1;
        
        if (weaponSlots[weaponSlotCorrected].slotFilled == true)
        {
           
            if (weaponSlots[weaponSlotCorrected].isItemEquipped != true)
            {

                if (currentHeldWeapon != weaponSlotCorrected)
                {
                    weaponSlots[currentHeldWeapon].isItemEquipped = false;
                    foreach (Transform child in weaponEquipParent.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                weaponSlots[weaponSlotCorrected].isItemEquipped = true;
                currentHeldWeapon = weaponSlotCorrected;

                if (weaponSlots[currentHeldWeapon].inventoryItem.itemPrefab != null)
                {
                    Debug.Log("Item Equiped called");

                    Transform weaponParent = weaponEquipParent.transform;
                    GameObject heldWeapon = Instantiate(weaponSlots[currentHeldWeapon].inventoryItem.itemPrefab);
                    heldWeapon.transform.SetParent(weaponParent, false);
                }
            }
        }
    }
}
