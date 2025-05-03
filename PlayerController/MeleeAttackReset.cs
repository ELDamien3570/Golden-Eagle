using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackReset : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackRightReset", true);
        
        
    }

}
