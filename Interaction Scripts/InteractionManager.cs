using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] 
    private InteractableGameObject currentTarget;
    public UIManager manager;   
    public PlayerInventoryManager inventoryManager;
    public LayerMask interactableLayer;
    
    [Header("Raycast Settings")]
    public float interactionDistance = 3f;   
    public float sphereRadius = .5f;
    public Camera playerCamera;

    [Header("Triggers")]
    public bool interacting = false;



    private void Start()
    {
        manager = FindFirstObjectByType<UIManager>();    
        inventoryManager = GetComponent<PlayerInventoryManager>();

    }

    void Update()
    {
        CheckForInteractable();
        InteractWithAim();       
    }
    public void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);   
        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereRadius, out hit, interactionDistance, interactableLayer))
        {          
            InteractableGameObject interactable = hit.collider.GetComponent<InteractableGameObject>();
            if (interactable != null && !inventoryManager.isInventoryFull)
            {            
                interacting = true;
                currentTarget = interactable;
                manager.InteractToolTip(interacting, currentTarget.interaction.promptText);                             
                return;
            }
        }
        interacting = false;
        currentTarget = null;
        manager.InteractToolTip(interacting, null);
    }
    public void InteractWithAim()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact(gameObject);
        }
    }
}
