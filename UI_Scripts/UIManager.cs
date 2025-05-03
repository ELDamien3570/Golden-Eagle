using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Objects")]
    public GameObject CharacterMenu;
    public GameObject InventoryMenu;
    public GameObject interactTooltip;

    [Header("Bool Triggers")]
    public bool inventoryOpen = false;
    public bool characterOpen = false;  

    public void Update()
    {
        ChangeCursorState();
    }
    public void ShowCharacterMenu()
    {
        ToggleCharacterMenu();
    }
    public void ShowInventoryMenu()
    {
        ToggleInventoryMenu();
    }

    private void ToggleInventoryMenu()
    {
        inventoryOpen = !inventoryOpen;
        InventoryMenu.SetActive(inventoryOpen);
    }

    private void ToggleCharacterMenu()
    {
        characterOpen= !characterOpen;
        CharacterMenu.SetActive(characterOpen);
    }

    public void InteractToolTip(bool tipState, string promptText)
    {
       // Debug.Log("Interacting");
        if (promptText != null)
        {
            ChangeInteractText(promptText);
            
        }
        interactTooltip.SetActive(tipState);
    }

    private void ChangeInteractText(string text)
    {
        TextMeshProUGUI interactText = interactTooltip.GetComponentInChildren<TextMeshProUGUI>();
        interactText.text = text;
    }
    private void ChangeCursorState()
    {
        if (inventoryOpen || characterOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
