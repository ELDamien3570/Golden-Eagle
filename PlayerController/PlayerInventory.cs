using UnityEngine;
using UnityEngine.UIElements;

namespace GE
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public GameObject inventory;
        public bool inventoryShowing;


        private void Awake()
        {
            
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        private void Update()
        {
            InventoryState();
        }

        public void InventoryState()
        {
            if (inventory.activeSelf)
            {
                inventoryShowing = true;    
            }
            else
            {
                inventoryShowing = false;
            }
        }

        public void OpenInventory()
        {
            inventory.SetActive(true);           
        }
    }
}
