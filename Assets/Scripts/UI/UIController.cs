using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance { get; private set; }

    public static bool isDataLoaded = false;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void SaveError(string error)
    {
        Debug.LogError(error);
    }

    public void LoadError(string error) 
    {
        Debug.LogError(error);
    }

    public void UISaveData()
    {
        if (SceneManager.GetActiveScene().name != "overworld") return;
        SaveManager.SaveData();
    }

    public void UILoadData()
    {
        SaveManager.FetchData();
    }

    public void continueGame()
    {
        if (!SaveManager.wasThereAnError)
        {
            SceneManager.LoadScene("overworld");
        }
    }

    public void startNewGame()
    {
        SceneManager.LoadScene("overworld");
        resetSave();
    }

    public void resetSave()
    {
        SaveManager.resetSave();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void debug_enter_dungeon()
    {
        SceneManager.LoadScene("dungeon");
    }

    public void ReturnToMenu()
    {
        if ( SceneManager.GetActiveScene().name == "overworld" )
        {
            UISaveData();
        }
        GAMEINITIALIZER.globalGameLevel = 1;
        JQUI.InventoryController.inventory = null;
        JQUI.InventoryController.armor = null;
        SceneManager.LoadScene("mainmenu");
    }
}