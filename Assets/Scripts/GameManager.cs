using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool menuOpened = false;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuOpened = !menuOpened;
            ShowInventory();
            ShowCraftingUI(menuOpened);
            ChestUI.instance.DisableChestContainer();

            // forcefully close menu just in case
            //if (!menuOpened)
            //    ShowCraftingUI(false);
        }
    }

    // called from PlayerInteracting script
    public void OpenChest(Chest chest)
    {
        menuOpened = !menuOpened;

        ShowInventory();
        ChestUI.instance.ShowChestUI(chest);

        CraftingUI.instance.EnableCraftingWindow(false);
    }
    
    public void ShowCraftingUI(bool enable)
    {
        CraftingUI.instance.EnableCraftingWindow(enable);
    }
    public void ShowInventory()
    {
        if (menuOpened)
        {
            UI.instance.ShowInventoryPanel(true);
        }
        else
        {
            UI.instance.ShowInventoryPanel(false);
        }
        if (isPaused)
        {
            isPaused = false;
            Resume();
        }
        else
        {
            isPaused = true;
            Pause();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
}
