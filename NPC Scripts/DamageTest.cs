using UnityEngine;

namespace GE
{
    public class DamageTest : MonoBehaviour
    {
        public int damage = 24;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if ( playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}
