using UnityEngine;

namespace GE
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
           if (other.tag == "Player")
           {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
           }
           if (other.tag == "enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }


        }
    }
}
