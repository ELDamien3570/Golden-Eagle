using UnityEngine;

namespace GE
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftDamageCollider;
        DamageCollider rightDamageCollider;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlots in weaponHolderSlots)
            {
                if(weaponSlots.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlots;
                }
                else if(weaponSlots.isRightHandSlot)
                {
                    rightHandSlot = weaponSlots;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                leftDamageCollider.EnableDamageCollider();
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);  
                LoadRightWeaponDamageCollider();
                rightDamageCollider.EnableDamageCollider();
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
           leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();   
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftDamageCollider()
        {
            leftDamageCollider.EnableDamageCollider();
        }
        public void OpenRightDamageCollider()
        {
            rightDamageCollider.EnableDamageCollider();
        }
        public void CloseLeftDamageCollider()
        {
            leftDamageCollider.DisableDamageCollider();
        }

        public void CloseRightDamageCollider()
        {
            rightDamageCollider.DisableDamageCollider();    
        }

        #endregion
    }
}

