using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GE
{
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls; //Input Manager
        AnimatorManager animatorManager;
        PlayerLocomotion playerLocomotion;
        PlayerEquipmentManager playerEquipmentManager;
        UIManager UIManager;

        //public GameObject inventoryUI;
        //public GameObject characterUI;

        public Vector2 movementInput; //Stores our vector2 from input manager for wasd
        public Vector2 cameraInput;

        [Header("Camera Inputs")]
        public float cameraInputX;
        public float cameraInputY;

        [Header("Movement Inputs")]
        public float moveAmount;
        public float verticalInput;
        public float horizontalInput;
        public bool sprintInput;
        public bool jumpInput;

        [Header("Action Inputs")]
        public bool slashInput;
        public bool blockInput;
        public bool stabInput;
        public bool overheadInput;
        public bool altSlashInput;
        public bool altOverheadInput;
        public bool specialInput;
        public bool throwInput;
        public bool kickInput;

        [Header("UI Inputs")]
        public bool inventoryInput;
        public bool characterInput;
        private bool isInventoryOpen = false;
        private bool isCharacterMenuOpen = false;

        [Header("Equip Actions")]
        public int equippedSlot = 0;

        [Header("Keybinds")]
        public KeyCode jumpKey = KeyCode.Space;

        private void Update()
        {

        }
        private void Awake()
        {
            UIManager = FindFirstObjectByType<UIManager>();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            animatorManager = GetComponent<AnimatorManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();

        }
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                playerControls.PlayerActions.Sprinting.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprinting.canceled += i => sprintInput = false;

                playerControls.PlayerActions.Slash.performed += i => slashInput = true;
                playerControls.PlayerActions.Slash.canceled += i => slashInput = false;

                playerControls.PlayerActions.Block.performed += i => blockInput = true;
                playerControls.PlayerActions.Block.canceled += i => blockInput = false;

                playerControls.PlayerActions.Stab.performed += i => stabInput = true;
                playerControls.PlayerActions.Stab.canceled += i => stabInput = false;

                playerControls.PlayerActions.Overhead.performed += i => overheadInput = true;
                playerControls.PlayerActions.Overhead.canceled += i => overheadInput = false;

                playerControls.PlayerActions.AltSlash.performed += i => altSlashInput = true;
                playerControls.PlayerActions.AltSlash.canceled += i => altSlashInput = false;

                playerControls.PlayerActions.AltOverhead.performed += i => altOverheadInput = true;
                playerControls.PlayerActions.AltOverhead.canceled += i => altOverheadInput = false;

                playerControls.PlayerActions.SpecialAttack.performed += i => specialInput = true;
                playerControls.PlayerActions.SpecialAttack.canceled += i => specialInput = false;

                playerControls.PlayerActions.Kick.performed += i => kickInput = true;
                playerControls.PlayerActions.Kick.canceled += i => kickInput = false;

                playerControls.PlayerActions.Throw.performed += i => throwInput = true;
                playerControls.PlayerActions.Throw.canceled += i => throwInput = false;

                playerControls.UIActions.OpenInventory.performed += HandleInventoryInput;
                playerControls.UIActions.OpenCharacter.performed += HandleCharacterMenuInput;

                playerControls.EquipActions.EquipWeapon1.performed += i => equippedSlot = 1;
                playerControls.EquipActions.EquipWeapon2.performed += i => equippedSlot = 2;
                playerControls.EquipActions.EquipWeapon3.performed += i => equippedSlot = 3;
                playerControls.EquipActions.EquipWeapon4.performed += i => equippedSlot = 4;

                playerControls.EquipActions.EquipWeapon1.performed += HandleEquipWeaponInput;
                playerControls.EquipActions.EquipWeapon2.performed += HandleEquipWeaponInput;
                playerControls.EquipActions.EquipWeapon3.performed += HandleEquipWeaponInput;
                playerControls.EquipActions.EquipWeapon4.performed += HandleEquipWeaponInput;


                if (Input.GetKey(jumpKey))
                {
                    jumpInput = true;
                }
                else
                {
                    jumpInput = false;
                }
            }

            playerControls.Enable();


        }
        private void OnDisable()
        {
            playerControls.Disable();
            playerControls.UIActions.OpenInventory.canceled -= i => inventoryInput = false;
        }
        public void HandleAllInputs()
        {
            #region Movement and Locomotion Inputs
            HandleMovementInput();
            HandleSprintingInput();
            HandleJumpInput();
            HandleSlashInput();
            HandleBlockInput();
            HandleStabInput();
            HandleOverheadInput();
            HandleSpecialInput();
            HandleKickInput();
            HandleAltSlashInput();
            HandleAltOverheadInput();
            HandleThrowInput();
            #endregion

            
            #region UI Controls
            // HandleInventoryInput();
            //HandleCharacterMenuInput();


            #endregion
        }
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            cameraInputY = cameraInput.y;
            cameraInputX = cameraInput.x;


            //moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            moveAmount = Mathf.Clamp((verticalInput), -1f, 1f);
            horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

            animatorManager.UpdateAnimatorValues(horizontalInput, moveAmount, playerLocomotion.isSprinting); //Replace horizontalInput with 0 to stop strafes
        }
        private void HandleSprintingInput()
        {
            if (sprintInput && moveAmount > 0.5f)
            {
                playerLocomotion.isSprinting = true;

            }
            else
            {
                playerLocomotion.isSprinting = false;
            }
        }
        private void HandleJumpInput()
        {
            //You have to put the resetjump script on jump animation element
            if (jumpInput)
            {
                Debug.Log("Jump");
                playerLocomotion.HandleJump();
            }
        }
        private void HandleSlashInput()
        {
            if (slashInput)
                playerLocomotion.HandleMeleeAttackSlash();
        }
        private void HandleBlockInput()
        {
            if (blockInput)
                playerLocomotion.HandleBlock();
        }
        private void HandleStabInput()
        {

            if (stabInput)
            {
                playerLocomotion.HandleStab();
            }
        }
        private void HandleOverheadInput()
        {
            if (overheadInput)
            {
                playerLocomotion.HandleOverhead();
            }
        }
        private void HandleSpecialInput()
        {
            if (specialInput)
            {

                playerLocomotion.HandleSpecial();
            }
        }
        private void HandleKickInput()
        {
            if (kickInput)
            {
                playerLocomotion.HandleKick();
            }
        }
        private void HandleAltSlashInput()
        {
            if (altSlashInput)
                playerLocomotion.HandleMeleeAttackAltSlash();
        }
        private void HandleAltOverheadInput()
        {
            if (altOverheadInput)
            {
                playerLocomotion.HandleAltOverhead();
            }
        }
        private void HandleThrowInput()
        {
            if (throwInput)
            {
                playerLocomotion.HandleThrow();
            }
        }

        private void HandleInventoryInput(InputAction.CallbackContext context)
        {
            UIManager.ShowInventoryMenu();
            isInventoryOpen = UIManager.inventoryOpen;
        }

        private void HandleCharacterMenuInput(InputAction.CallbackContext context)
        {
            UIManager.ShowCharacterMenu();
            isCharacterMenuOpen = UIManager.characterOpen;
        }

        private void HandleEquipWeaponInput(InputAction.CallbackContext context)
        {
            if (equippedSlot == 0)
            {
                return;
            }
            else if (equippedSlot != 0)
            {
                playerEquipmentManager.EquipWeapon(equippedSlot);
            }

        }

    }   
}
