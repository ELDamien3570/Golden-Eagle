using UnityEngine;

namespace GE
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        Animator animator;
       
        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;           
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            
            animator.Play("GetHit");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death");
                //Handle player death, reload or end game
            }
        }

    }
}
