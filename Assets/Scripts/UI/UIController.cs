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
    }

    public void SaveError(string error)
    {

    }

    public void LoadError(string error) 
    {

    }

    public void UISaveData()
    {
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

    }

    public void resetSave()
    {
        SaveManager.resetSave();
    }
}