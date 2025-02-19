using UnityEngine;

namespace GE
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        public int damage;
        public int animationSpeed;
        public AnimatorOverrideController weaponClass;
    }
}
