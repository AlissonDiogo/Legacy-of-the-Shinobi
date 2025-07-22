using UnityEngine;
using UnityEngine.InputSystem;

public class StatusWindowController : MonoBehaviour
{
    [SerializeField] private GameObject statusPanel;

    private UIWindows uiWindows;

    private void Awake()
    {
        uiWindows = new UIWindows();
    }

    private void OnEnable()
    {
        uiWindows.UIManager.Enable();
        uiWindows.UIManager.StatusWindow.performed += ToggleStatusWindow;
    }

    private void OnDisable()
    {
        uiWindows.UIManager.StatusWindow.performed -= ToggleStatusWindow;
        uiWindows.UIManager.Disable();
    }

    private void ToggleStatusWindow(InputAction.CallbackContext context)
    {
        statusPanel.SetActive(!statusPanel.activeSelf);
    }
}
