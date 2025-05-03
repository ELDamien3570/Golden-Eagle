using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace GE
{
    public class PlayerManager : MonoBehaviour
    {
        Animator animator;
        InputManager inputManager;
        CameraManager cameraManager;
        PlayerLocomotion playerLocomotion;
        

        public bool isInteracting;

        private void Awake()
        {
            
            animator = GetComponent<Animator>();
            inputManager = GetComponent<InputManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            cameraManager = FindAnyObjectByType<CameraManager>();
        }

        public void Update()
        {
            inputManager.HandleAllInputs();

            
        }
        private void FixedUpdate()
        {
            playerLocomotion.HandleAllMovement();
        }

        private void LateUpdate()
        {
            cameraManager.HandleAllCameraMovement();

            isInteracting = animator.GetBool("isInteracting");
            playerLocomotion.isJumping = animator.GetBool("isJumping");
            animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        }

     
    }
}
