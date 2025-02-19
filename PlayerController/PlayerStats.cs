using JetBrains.Annotations;
using UnityEngine;

namespace GE
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        AnimatorManager animatorManager;
        public HealthBar healthbar;
        
        void Start()
        {     
            animatorManager = GetComponent<AnimatorManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth(currentHealth);
            animatorManager.PlayTargetAnimation("GetHit", true);

            if (currentHealth <= 0) 
            {
                currentHealth = 0;
                animatorManager.PlayTargetAnimation("Death", true);
                //Handle player death, reload or end game
            }
        }

    }
}
