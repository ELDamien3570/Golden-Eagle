using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Animations;
using UnityEngine.InputSystem.HID;

namespace GE
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        AnimatorManager animatorManager;
        InputManager inputManager;


        Vector3 moveDirection;
        Transform cameraObject;
        Rigidbody playerRigidbody;

        [Header("Falling")]
        public float inAirTimer;
        public float fallingVelocity;
        public float leapingVelocity;
        public LayerMask groundLayer;
        public float rayCastHeightOffset = 0.5f;

        [Header("Movement Flages")]
        public bool isSprinting;
        public bool isGrounded;
        public bool isJumping;

        [Header("Movement Speeds")]
        public float walkingSpeed = 1.5f;
        public float runningSpeed = 5;
        public float sprintingSpeed = 7;
        public float rotationSpeed = 15;

        [Header("Jump Speeds")]
        public float jumpHeight = 1;
        public float gravityIntensity = -15;

        [Header("Real Action Checks")]
        public bool isSwinging;
        public bool justJumped = false;
        public bool justSlashed = false;
        public bool realGrounded;
        public bool gotHit = false; //Use later with gethit funtion to determine if player got hit and play animation if they did

        [Header("Keycodes")]
        public KeyCode freeLookKey = KeyCode.LeftAlt;
        public KeyCode jumpKey = KeyCode.Space;


        private void Awake()
        {

            animatorManager = GetComponent<AnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            inputManager = GetComponent<InputManager>();
            playerRigidbody = GetComponent<Rigidbody>();

            cameraObject = Camera.main.transform;
            isGrounded = true;
        }

        public void Update()
        {
            RaycastHit reallyHit;
            float rayCastHeightCheck = 0.5f;
            Vector3 rayCastOrigin = transform.position;
            rayCastOrigin.y = rayCastOrigin.y + rayCastHeightCheck;
            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out reallyHit, .5f, groundLayer))
            {
                realGrounded = true;
                playerManager.isInteracting = true;
            }
            else
            {
                realGrounded = false;

            }

        } 
        private void LateUpdate()
        {
            isSwinging = false;
           
        }
        public void HandleAllMovement()
        {

            if (playerManager.isInteracting)
            {
                return;
            }
            HandleFallingAndLanding();
            HandleMovement();
            HandleRotation();
            HandleJump();
        }

        private void HandleMovement()
        {


            if (isJumping)
                return;

            if (!realGrounded)
            {
                return;
            }



            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;

            //moveDirection = cameraObject.forward * inputManager.verticalInput;
            //moveDirection = moveDirection + Vector3.one * inputManager.horizontalInput;


            moveDirection.y = 0;

            moveDirection.Normalize();


            if (isSprinting)
            {
                moveDirection = moveDirection * sprintingSpeed;
            }
            else
            {
                if (inputManager.moveAmount >= 0.5f)
                {
                    moveDirection = moveDirection * runningSpeed;
                }
                else
                {
                    moveDirection = moveDirection * walkingSpeed;
                }
            }

            Vector3 movementVelocity = moveDirection;
            playerRigidbody.linearVelocity = movementVelocity;

        }
        private void HandleRotation()
        {
            Vector3 targetDirection = Vector3.zero;
            Vector3 one = Vector3.one;

            if (!Input.GetKey(freeLookKey))
                targetDirection = cameraObject.forward;
            else
            {
                targetDirection = cameraObject.forward * inputManager.verticalInput;

                targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
            }


            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
        private void HandleFallingAndLanding()
        {
            RaycastHit hit;

            Vector3 rayCastOrigin = transform.position;
            rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
            //float maxDistance = .5f;
            if (!isGrounded && !isJumping)
            {
                if (!playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Falling", true);
                    Debug.Log("Falling");
                    isJumping = false;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                playerRigidbody.AddForce(transform.forward * leapingVelocity);
                playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            }
            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, .8f, groundLayer))
            {
                if (!isGrounded && !playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Land", true);
                    Debug.Log("Landing");
                    isJumping = false;

                }
                inAirTimer = 0;
                isGrounded = true;

                playerManager.isInteracting = false;
            }
            else
            {
                isGrounded = false;
            }

            
        }
        public void HandleJump()
        {
            if (isGrounded && Input.GetKey(jumpKey) && justJumped == false)
            {
                Debug.Log("JustJumped");
                StartCoroutine(justJumpedCheck());
                justJumped = true;
                animatorManager.animator.SetBool("isJumping", true);
                animatorManager.PlayTargetAnimation("Unarmed-Jump", false);

                float jumpingVelocity = Mathf.Sqrt(-4 * gravityIntensity * jumpHeight);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpingVelocity;
                playerRigidbody.linearVelocity = playerVelocity;


            }
            else
            {
                isJumping = false;
                //animatorManager.animator.SetBool("isJumping", false);
            }
        }

        public void HandleMeleeAttackSlash()
        {
            isSwinging = true;
            if (!justSlashed)
            {              
                animatorManager.PlayTargetAnimation("MeleeAttack1", false);
                justSlashed = true;

            }
            else if (justSlashed)
            {
                animatorManager.PlayTargetAnimation("MeleeAttack2", false);
                justSlashed = false;

            }
            StartCoroutine(IsSwingingCheck());
        }

        public void HandleBlock()
        {

            animatorManager.PlayTargetAnimation("Block", false);
            if (gotHit)
            {
              
                animatorManager.PlayTargetAnimation("GetHit", false);
            }
        }

        public void HandleStab()
        {
            isSwinging = true;
            if (!justSlashed)
            {
                animatorManager.animator.SetBool("AttackRightReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack3", false);
                justSlashed = true;
            }
            else if (justSlashed)
            {
                animatorManager.animator.SetBool("AttackLeftReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack4", false);
                justSlashed = false;

            }
            StartCoroutine(IsSwingingCheck()); 
        }
        public void HandleOverhead()
        {
            isSwinging = true;
            if (!justSlashed)
            {
                animatorManager.animator.SetBool("AttackRightReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack5", false);
                justSlashed = true;
            }
            else if (justSlashed)
            {
                animatorManager.animator.SetBool("AttackLeftReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack6", false);
                justSlashed = false;

            }
            StartCoroutine(IsSwingingCheck());
        }
        public void HandleSpecial()
        {
            isSwinging = true;
            if (!justSlashed)
            {
                animatorManager.animator.SetBool("AttackRightReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack7", false);
                justSlashed = true;
            }
            else if (justSlashed)
            {
                animatorManager.animator.SetBool("AttackLeftReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack6", false);
                justSlashed = false;

            }
            StartCoroutine(IsSwingingCheck());
        }
        public void HandleKick()
        {
            isSwinging = true;

            animatorManager.PlayTargetAnimation("MeleeAttack8", true);
            justSlashed = true;

            StartCoroutine(IsSwingingCheck());
        }

        public void HandleMeleeAttackAltSlash()
        {
            isSwinging = true;
            if (!justSlashed)
            {
                animatorManager.animator.SetBool("AttackLeftReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack2", false);
                justSlashed = true;
            }
            else if (justSlashed)
            {
                animatorManager.animator.SetBool("AttackRightReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack1", false);
                justSlashed = false;
            }
            StartCoroutine(IsSwingingCheck());
        }
        public void HandleAltOverhead()
        {
            isSwinging = true;
            if (!justSlashed)
            {
                animatorManager.animator.SetBool("AttackLeftReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack6", false);
                justSlashed = true;
            }
            else if (justSlashed)
            {
                animatorManager.animator.SetBool("AttackRightReset", false);
                animatorManager.PlayTargetAnimation("MeleeAttack5", false);
                justSlashed = false;

            }
            StartCoroutine(IsSwingingCheck());
        }

        public void HandleThrow()
        {
            isSwinging = true;
            animatorManager.animator.SetBool("AttackRightReset", false);
            animatorManager.PlayTargetAnimation("Throw", false);
            justSlashed = true;


            StartCoroutine(IsSwingingCheck());
        }

        private IEnumerator justJumpedCheck()
        {
            yield return new WaitForSeconds(1.5f);
            justJumped = false;
        }
        private IEnumerator IsSwingingCheck()
        {
            yield return new WaitForSeconds(3);
            isSwinging = false;
        }
    }
}
