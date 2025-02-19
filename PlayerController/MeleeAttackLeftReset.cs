using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackLeftReset : StateMachineBehaviour
{
   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackLeftReset", true);
        
    }

    
}
