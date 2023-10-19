using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject PauseMenuParent;
    public void ToggleGame()
    {
        if (isGamePaused)
        {
            closePauseMenu();
        }
        else
        {
            if (JQUI.InventoryController.isOpen)
                JQUI.InventoryController.instance.CloseInventory();
            else openPauseMenu();
        }
    }

    private void Awake()
    {
        isGamePaused = false;
    }

    void openPauseMenu()
    {
        PauseMenuParent.SetActive(true);
        isGamePaused = true;
    }

    void closePauseMenu()
    {
        PauseMenuParent.SetActive(false);
        isGamePaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGame();
        }

        Time.timeScale = Mathf.Lerp(Time.timeScale, isGamePaused ? 0f : 1f, Time.deltaTime * 10f);
    }

    public void exitToMenu()
    {
        UIController.instance.ReturnToMenu();
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
